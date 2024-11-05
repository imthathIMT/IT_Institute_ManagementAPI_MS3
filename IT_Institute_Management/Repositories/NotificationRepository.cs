using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace IT_Institute_Management.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly InstituteDbContext _context;

        public NotificationRepository(InstituteDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(Guid id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteNotificationAsync(Guid id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Notification not found.");
            }
        }
    }
}
