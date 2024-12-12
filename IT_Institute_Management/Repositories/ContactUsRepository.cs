using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System;

namespace IT_Institute_Management.Repositories
{
    public class ContactUsRepository : IContactUsRepository
    {
        private readonly InstituteDbContext _instituteDbContext;
        public ContactUsRepository(InstituteDbContext context)
        {
            _instituteDbContext = context;
        }
        public async Task<IEnumerable<ContactUs>> GetAllAsync()
        {
            return await _instituteDbContext.ContactUs.ToListAsync();
        }
        public async Task<ContactUs> GetByIdAsync(Guid id)
        {
            return await _instituteDbContext.ContactUs.FindAsync(id);
        }


        public async Task AddAsync(ContactUs contactUs)
        {
            await _instituteDbContext.ContactUs.AddAsync(contactUs);
            await _instituteDbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(ContactUs contactUs)
        {
            _instituteDbContext.ContactUs.Update(contactUs);
            await _instituteDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var contactUs = await _instituteDbContext.ContactUs.FindAsync(id);
            if (contactUs != null)
            {
                _instituteDbContext.ContactUs.Remove(contactUs);
                await _instituteDbContext.SaveChangesAsync();
            }
        }


        public async Task<ContactUs> GetByEmail(string email)
        {
            var enquiry = await _instituteDbContext.ContactUs.FirstOrDefaultAsync(x => x.Email == email);
            if (enquiry == null)
            {
                throw new Exception("Enquiry not found");
            }

            return enquiry;
        }


    }
}
