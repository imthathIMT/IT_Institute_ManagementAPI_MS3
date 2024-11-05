using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification> GetNotificationByIdAsync(Guid id);
        Task AddNotificationAsync(Notification notification);
        Task UpdateNotificationAsync(Notification notification);
        Task DeleteNotificationAsync(Guid id);
    }
}
