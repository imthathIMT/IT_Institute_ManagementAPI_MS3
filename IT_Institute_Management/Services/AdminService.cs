using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;

namespace IT_Institute_Management.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserService _userService;
        public AdminService(IAdminRepository adminRepository,IUserService userService)
        {
            _adminRepository = adminRepository;
            _userService = userService;
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
            var admin = new Admin { NIC = adminDto.NIC, Password = adminDto.Password };
            await _adminRepository.AddAsync(admin);
        }
        public async Task UpdateAsync(AdminRequestDto adminDto)
        {
            var admin = new Admin { NIC = adminDto.NIC, Password = adminDto.Password };
            await _adminRepository.UpdateAsync(admin);
        }
        public async Task DeleteAsync(string nic)
        {
            await _adminRepository.DeleteAsync(nic);
        }



    }
}
