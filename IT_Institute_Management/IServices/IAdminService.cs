using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface IAdminService
    {
        Task<IEnumerable<AdminResponseDto>> GetAllAsync();
        Task<AdminResponseDto> GetByIdAsync(string nic);
        Task AddAsync(AdminRequestDto adminDto);
        Task UpdateAsync(AdminRequestDto adminDto);
        Task DeleteAsync(string nic);
    }
}
