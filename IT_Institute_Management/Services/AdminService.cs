using IT_Institute_Management.Database;
using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using IT_Institute_Management.PasswordService;
using Microsoft.EntityFrameworkCore;

namespace IT_Institute_Management.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly InstituteDbContext _instituteDbContext;
        public AdminService(IAdminRepository adminRepository,IUserService userService, IPasswordHasher passwordHasher, InstituteDbContext instituteDbContext)
        {
            _adminRepository = adminRepository;
            _userService = userService;
            _passwordHasher = passwordHasher;
            _instituteDbContext = instituteDbContext;
        }
        public async Task<IEnumerable<AdminResponseDto>> GetAllAsync()
        {
            try
            {
                var admins = await _adminRepository.GetAllAsync();
                return admins.Select(a => new AdminResponseDto
                {
                    NIC = a.NIC,
                    Name = a.Name,
                    Email = a.Email,
                    Phone = a.Phone
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving admins.", ex);
            }
        }

        public async Task<AdminResponseDto> GetByIdAsync(string nic)
        {
            try
            {
                var admin = await _adminRepository.GetByIdAsync(nic);
                if (admin == null)
                {
                    throw new KeyNotFoundException($"Admin with NIC {nic} not found.");
                }

                return new AdminResponseDto
                {
                    NIC = admin.NIC,
                    Name = admin.Name,
                    Email = admin.Email,
                    Phone = admin.Phone
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving the admin.", ex);
            }
        }


        public async Task AddAsync(AdminRequestDto adminDto)
        {
            try
            {
                // Check if the NIC already exists in the Users table
                var existingUser = await _instituteDbContext.Users
                    .FirstOrDefaultAsync(u => u.NIC == adminDto.NIC);

                if (existingUser != null)
                {
                    throw new ApplicationException($"A user with the NIC {adminDto.NIC} already exists.");
                }

                // Hash password before saving
                var hashedPassword = _passwordHasher.HashPassword(adminDto.Password);

                // Create the User entity for the Admin
                var user = new User()
                {
                    NIC = adminDto.NIC,
                    Password = hashedPassword,  // Store hashed password
                    Role = Role.Admin
                };

                // Add the User entity to the Users DbSet
                await _instituteDbContext.Users.AddAsync(user);
                await _instituteDbContext.SaveChangesAsync();  // Save the User first to generate the UserId

                // Now create the Admin entity and link it to the User via UserId
                var admin = new Admin
                {
                    NIC = adminDto.NIC,
                    Name = adminDto.Name,
                    Phone = adminDto.Phone,
                    Email = adminDto.Email,
                    Password = hashedPassword, // Store hashed password
                    UserId = user.Id
                };

                // Add the Admin entity to the Admins DbSet
                await _instituteDbContext.Admins.AddAsync(admin);
                await _instituteDbContext.SaveChangesAsync(); // Save the Admin entity
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while adding the admin.", ex);
            }
        }




        public async Task UpdateAsync(AdminRequestDto adminDto)
        {
            try
            {
                var admin = await _adminRepository.GetByIdAsync(adminDto.NIC);
                if (admin == null)
                {
                    throw new KeyNotFoundException($"Admin with NIC {adminDto.NIC} not found.");
                }

                // If password is provided, hash and update it
                if (!string.IsNullOrEmpty(adminDto.Password))
                {
                    admin.Password = _passwordHasher.HashPassword(adminDto.Password);
                    await _userService.UpdateAsync(adminDto.NIC, new UserRequestDto { Password = adminDto.Password });
                }

                admin.Name = adminDto.Name;
                admin.Phone = adminDto.Phone;
                admin.Email = adminDto.Email;

                // Start a transaction to update both Admin and User tables
                using (var transaction = await _instituteDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Update Admin table
                         _instituteDbContext.Admins.Update(admin);

                        // Update User table
                        var user = await _instituteDbContext.Users.FirstOrDefaultAsync(u => u.NIC == adminDto.NIC);
                        if (user != null)
                        {
                            user.Password = admin.Password;
                            user.Role = Role.Admin;
                            _instituteDbContext.Users.Update(user);
                        }

                        await _instituteDbContext.SaveChangesAsync();

                        // Commit transaction if both updates succeed
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an error occurs
                        await transaction.RollbackAsync();
                        throw new ApplicationException("An error occurred while updating the admin.", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the admin.", ex);
            }
        }



        public async Task DeleteAsync(string nic)
        {
            try
            {
                var admin = await _instituteDbContext.Admins
                    .FirstOrDefaultAsync(a => a.NIC == nic);

                if (admin == null)
                {
                    throw new KeyNotFoundException($"Admin with NIC {nic} not found.");
                }

                // Remove the Admin entity
                _instituteDbContext.Admins.Remove(admin);

                // Also delete the associated User entity
                var user = await _instituteDbContext.Users
                    .FirstOrDefaultAsync(u => u.NIC == nic);
                if (user != null)
                {
                    _instituteDbContext.Users.Remove(user);
                }

                await _instituteDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while deleting the admin.", ex);
            }
        }



    }
}
