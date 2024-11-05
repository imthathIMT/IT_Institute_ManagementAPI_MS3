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

    }
}
