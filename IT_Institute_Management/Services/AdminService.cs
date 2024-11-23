using IT_Institute_Management.Database;
using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.ImageService;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using IT_Institute_Management.PasswordService;
using IT_Institute_Management.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IT_Institute_Management.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly InstituteDbContext _instituteDbContext;
        private readonly IImageService _imageService;
        public AdminService(IAdminRepository adminRepository, IUserService userService, IPasswordHasher passwordHasher, InstituteDbContext instituteDbContext, IImageService imageService)
        {
            _adminRepository = adminRepository;
            _userService = userService;
            _passwordHasher = passwordHasher;
            _instituteDbContext = instituteDbContext;
            _imageService = imageService;
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
                    Phone = a.Phone,
                    ImagePath = a.ImagePath,
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
                var admin = await _adminRepository.GetByNIC(nic);
                if (admin == null)
                {
                    throw new KeyNotFoundException($"Admin with NIC {nic} not found.");
                }

                return new AdminResponseDto
                {
                    NIC = admin.NIC,
                    Name = admin.Name,
                    Email = admin.Email,
                    Phone = admin.Phone,
                    ImagePath = admin.ImagePath
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving the admin.", ex);
            }
        }



        public async Task<string> AddAsync(AdminRequestDto adminDto)
        {
            try
            {

                var existingUser = await _instituteDbContext.Users
                    .FirstOrDefaultAsync(u => u.NIC == adminDto.NIC);

                if (existingUser != null)
                {
                    throw new ApplicationException($"A user with the NIC {adminDto.NIC} already exists.");
                }


                var hashedPassword = _passwordHasher.HashPassword(adminDto.Password);

                var imagePath = string.Empty;

                if (adminDto.Image != null)
                {

                    imagePath = await _imageService.SaveImage(adminDto.Image, "admins");
                }

                var admin = new Admin
                {
                    NIC = adminDto.NIC,
                    Name = adminDto.Name,
                    Phone = adminDto.Phone,
                    Email = adminDto.Email,
                    Password = hashedPassword,
                    ImagePath = imagePath
                };


                await _userService.AddAsync(new UserRequestDto
                {
                    NIC = adminDto.NIC,
                    Password = hashedPassword
                }, Role.Admin);


                await _adminRepository.AddAsync(admin);
                return ("Admin Successfully created");
            }

            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while adding the admin.", ex);
            }
        }




        public async Task<string> UpdateAsync(AdminRequestDto adminDto)
        {

            var admin = await _adminRepository.GetByNIC(adminDto.NIC);
            if (admin == null)
            {
                throw new KeyNotFoundException($"Admin with NIC {adminDto.NIC} not found.");
            }


            if (!string.IsNullOrEmpty(adminDto.Password))
            {
                admin.Password = _passwordHasher.HashPassword(adminDto.Password);
                await _userService.UpdateAsync(adminDto.NIC, new UserRequestDto { Password = adminDto.Password });
            }


            if (adminDto.Image != null)
            {

                if (!string.IsNullOrEmpty(admin.ImagePath))
                {
                    _imageService.DeleteImage(admin.ImagePath);
                }


                admin.ImagePath = await _imageService.SaveImage(adminDto.Image, "admins");

            }
            admin.NIC = adminDto.NIC;
            admin.Name = adminDto.Name;
            admin.Phone = adminDto.Phone;
            admin.Email = adminDto.Email;

            await _adminRepository.UpdateAsync(admin);
            return ("Admin Successfully Updated");
        }



        public async Task<string> DeleteAsync(string nic)
        {
            var admin = await _adminRepository.GetByNIC(nic);

            if (admin == null)
            {
                throw new Exception($"Admin with NIC {nic} not found.");
            }

            if (!string.IsNullOrEmpty(admin.ImagePath))
            {
                _imageService.DeleteImage(admin.ImagePath);
            }


            _adminRepository.DeleteAsync(nic);
            _userService.DeleteAsync(nic);
            return ("Admin Successfully deleted");



        }



    }
}
