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
        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public async Task<IEnumerable<AdminResponseDto>> GetAllAsync()
        {
            var admins = await _adminRepository.GetAllAsync();
            return admins.Select(a =>
            new AdminResponseDto { NIC = a.NIC, Password = a.Password });
        }
       
    }
}
