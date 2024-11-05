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
                Date = a.Date 
            }); 
        }
        public async Task<AnnouncementResponseDto> GetByIdAsync(Guid id)
        {
            var announcement = await _announcementRepository.GetByIdAsync(id);
            return new AnnouncementResponseDto
            {
                Id = announcement.Id,
                Title = announcement.Title,
                Body = announcement.Body,
                Date = announcement.Date
            };
        }
        public async Task AddAsync(AnnouncementRequestDto announcementDto)
        {
            var announcement = new Announcement
            {
                Id = Guid.NewGuid(),
                Title = announcementDto.Title,
                Body = announcementDto.Body,
                Date = announcementDto.Date
            };
            await _announcementRepository.AddAsync(announcement);
        }
        public async Task UpdateAsync(Guid id, AnnouncementRequestDto announcementDto)
        {
            var announcement = new Announcement
            {
                Id = id,
                Title = announcementDto.Title,
                Body = announcementDto.Body,
                Date = announcementDto.Date
            };
            await _announcementRepository.UpdateAsync(announcement);
        }


    }
}
