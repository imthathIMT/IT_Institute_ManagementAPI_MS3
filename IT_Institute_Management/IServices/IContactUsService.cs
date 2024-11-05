using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface IContactUsService
    {
        Task<IEnumerable<ContactUsResponseDto>> GetAllAsync();
    }
}
