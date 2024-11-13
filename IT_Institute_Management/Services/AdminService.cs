using IT_Institute_Management.Database;
using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using Microsoft.EntityFrameworkCore;

namespace IT_Institute_Management.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserService _userService;
        private readonly InstituteDbContext _instituteDbContext;
        public AdminService(IAdminRepository adminRepository,IUserService userService, InstituteDbContext instituteDbContext)
        {
            _adminRepository = adminRepository;
            _userService = userService;
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

                // Create a new User entity from the AdminRequestDto
                var user = new User()
                {
                    NIC = adminDto.NIC,
                    Password = adminDto.Password,
                    Role = Role.Admin
                };

                // Add the User entity to the Users DbSet
                await _instituteDbContext.Users.AddAsync(user);
                await _instituteDbContext.SaveChangesAsync(); // Save the User first to generate the UserId

                // Now create the Admin entity and set the UserId
                var admin = new Admin
                {
                    NIC = adminDto.NIC,
                    Name = adminDto.Name,
                    Phone = adminDto.Phone,
                    Email = adminDto.Email,
                    Password = adminDto.Password,
                    UserId = user.Id // Link the Admin to the User via UserId
                };

                // Add the Admin entity to the Admins DbSet
                await _instituteDbContext.Admins.AddAsync(admin);
                await _instituteDbContext.SaveChangesAsync(); // Save the Admin
            }
            catch (DbUpdateException dbEx)
            {
                // Handle errors related to database updates (e.g., constraint violations, foreign key issues)
                // Log the inner exception for debugging purposes
                // _logger.LogError(dbEx, "Error occurred while saving entity changes.");
                throw new ApplicationException($"Database error occurred while adding the admin: {dbEx.InnerException?.Message ?? dbEx.Message}", dbEx);
            }
            catch (ApplicationException appEx)
            {
                // Handle specific application exceptions (e.g., duplicate NIC)
                throw new ApplicationException($"Application error occurred while adding the admin: {appEx.Message}", appEx);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                // You can log the error to a logging service like Serilog or NLog (optional)
                // _logger.LogError(ex, "Unexpected error occurred while adding the admin.");
                throw new ApplicationException($"An unexpected error occurred while adding the admin: {ex.Message}", ex);
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

                admin.Name = adminDto.Name;
                admin.Password = adminDto.Password;

                await _adminRepository.UpdateAsync(admin);
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
                // Find the admin by NIC
                var admin = await _instituteDbContext.Admins
                    .FirstOrDefaultAsync(a => a.NIC == nic);

                if (admin == null)
                {
                    throw new KeyNotFoundException($"Admin with NIC {nic} not found.");
                }

                // Find the associated user
                var user = await _instituteDbContext.Users
                    .FirstOrDefaultAsync(u => u.NIC == nic);

                if (user == null)
                {
                    throw new KeyNotFoundException($"User with NIC {nic} not found.");
                }

                // Remove Admin and User
                _instituteDbContext.Admins.Remove(admin);
                _instituteDbContext.Users.Remove(user);

                // Save changes to the database
                await _instituteDbContext.SaveChangesAsync();
            }
            catch (KeyNotFoundException knfEx)
            {
                // Handle not found exceptions
                throw new ApplicationException(knfEx.Message, knfEx);
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update issues
                throw new ApplicationException("Database error occurred while deleting the admin.", dbEx);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                throw new ApplicationException($"Unexpected error occurred: {ex.Message}", ex);
            }
        }



    }
}
