using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace IT_Institute_Management.Repositories
{
    public class AnnouncementRepository :IAnnouncementRepository
    {
        private readonly InstituteDbContext _instituteDbcontext;
        public AnnouncementRepository(InstituteDbContext context) {
            _instituteDbcontext = context;
        }
        public async Task<IEnumerable<Announcement>> GetAllAsync() {
            return await _instituteDbcontext.Announcements.ToListAsync();
        }
        public async Task<Announcement> GetByIdAsync(Guid id) { 
            return await _instituteDbcontext.Announcements.FindAsync(id);
        }
        public async Task AddAsync(Announcement announcement) {
            await _instituteDbcontext.Announcements.AddAsync(announcement);
            await _instituteDbcontext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Announcement announcement) {
            _instituteDbcontext.Announcements.Update(announcement);
            await _instituteDbcontext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id) {
            var announcement = await _instituteDbcontext.Announcements.FindAsync(id);
            if (announcement != null) {
                _instituteDbcontext.Announcements.Remove(announcement);
                await _instituteDbcontext.SaveChangesAsync();
            } 
        }
    }
}
