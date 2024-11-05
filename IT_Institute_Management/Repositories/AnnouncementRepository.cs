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
       
    }
}
