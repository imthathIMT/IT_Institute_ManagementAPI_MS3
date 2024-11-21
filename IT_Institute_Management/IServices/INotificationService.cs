using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationResponseDTO>> GetAllNotificationsAsync();
        Task<NotificationResponseDTO> GetNotificationByIdAsync(Guid id);
        Task CreateNotificationAsync(NotificationRequestDTO notificationRequest);
        Task UpdateNotificationAsync(Guid id, NotificationRequestDTO notificationRequest);
        Task DeleteNotificationAsync(Guid id);
        Task SendNotificationAsync(string studentNIC, string message);
    }
}
