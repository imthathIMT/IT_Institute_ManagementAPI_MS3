using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface IAnnouncementService
    {
        Task<IEnumerable<AnnouncementResponseDto>> GetAllAsync();

        Task<AnnouncementResponseDto> GetByIdAsync(Guid id);
        Task AddAsync(AnnouncementRequestDto announcementDto);

    }
}
