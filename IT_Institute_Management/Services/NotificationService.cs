using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;

namespace IT_Institute_Management.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }


        public async Task<IEnumerable<NotificationResponseDTO>> GetAllNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetAllNotificationsAsync();
            return notifications.Select(n => new NotificationResponseDTO
            {
                Id = n.Id,
                Message = n.Message,
                Date = n.Date,
                StudentNIC = n.StudentNIC
            });
        }

        public async Task<NotificationResponseDTO> GetNotificationByIdAsync(Guid id)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(id);
            if (notification == null)
                throw new KeyNotFoundException("Notification not found.");

            return new NotificationResponseDTO
            {
                Id = notification.Id,
                Message = notification.Message,
                Date = notification.Date,
                StudentNIC = notification.StudentNIC
            };
        }


        public async Task CreateNotificationAsync(NotificationRequestDTO notificationRequest)
        {
            var notification = new Notification
            {
                Message = notificationRequest.Message,
                Date = notificationRequest.Date,
                StudentNIC = notificationRequest.StudentNIC
            };

            await _notificationRepository.AddNotificationAsync(notification);
        }
    }
}
