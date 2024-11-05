using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;

namespace IT_Institute_Management.Services
{
    public class AnnouncementService:IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;
        public AnnouncementService(IAnnouncementRepository announcementRepository) 
        {
            _announcementRepository = announcementRepository;
        }
        public async Task<IEnumerable<AnnouncementResponseDto>> GetAllAsync()
        {
            var announcements = await _announcementRepository.GetAllAsync();
            return announcements.Select(a => new AnnouncementResponseDto { 
                Id = a.Id,
                Title = a.Title,
                Body = a.Body,
                Date = a.Date }); 
        }
        
    }
}
