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
            return await _context.Notification.Include(s=>s.Student).ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(Guid id)
        {
            return await _context.Notification.FirstOrDefaultAsync(n => n.Id == id);
        }



        public async Task AddNotificationAsync(Notification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            await _context.Notification.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            _context.Notification.Update(notification);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteNotificationAsync(Guid id)
        {
            var notification = await _context.Notification.FindAsync(id);
            if (notification != null)
            {
                _context.Notification.Remove(notification);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Notification not found.");
            }
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsByStudentNicAsync(string studentNic)
        {
            return await _context.Notification
                                 .Where(n => n.StudentNIC == studentNic)
                                 .ToListAsync();
        }



        public async Task<bool> NotificationExistsAsync(Guid id)
        {
            return await _context.Notification.AnyAsync(n => n.Id == id);
        }
    }
}
