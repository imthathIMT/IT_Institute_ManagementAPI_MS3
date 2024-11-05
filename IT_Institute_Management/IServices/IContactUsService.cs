using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface IContactUsService
    {
        Task<IEnumerable<ContactUsResponseDto>> GetAllAsync();
        Task<ContactUsResponseDto> GetByIdAsync(Guid id);
        Task AddAsync(ContactUsRequestDto contactUsDto);
        Task UpdateAsync(Guid id, ContactUsRequestDto contactUsDto);
        Task DeleteAsync(Guid id);
    }
}
