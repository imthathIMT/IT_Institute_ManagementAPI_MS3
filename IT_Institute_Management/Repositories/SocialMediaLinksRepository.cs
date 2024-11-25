using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace IT_Institute_Management.Repositories
{
    public class SocialMediaLinksRepository: ISocialMediaLinksRepository
    {
        private readonly InstituteDbContext _context;

        public SocialMediaLinksRepository(InstituteDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SocialMediaLinks>> GetAllAsync()
        {
            return await _context.SocialMediaLinks.ToListAsync();
        }

        public async Task<SocialMediaLinks> GetByNICAsync(string nic)
        {
            return await _context.SocialMediaLinks.FirstOrDefaultAsync(s => s.StudentNIC == nic);
        }

        public async Task<SocialMediaLinks> CreateAsync(SocialMediaLinks socialMediaLinks)
        {
            _context.SocialMediaLinks.Add(socialMediaLinks);
            await _context.SaveChangesAsync();
            return socialMediaLinks;
        }

        public async Task<SocialMediaLinks> UpdateAsync(SocialMediaLinks socialMediaLinks)
        {
            _context.SocialMediaLinks.Update(socialMediaLinks);
            await _context.SaveChangesAsync();
            return socialMediaLinks;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.SocialMediaLinks.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.SocialMediaLinks.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
