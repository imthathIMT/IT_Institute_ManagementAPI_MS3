using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;
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


    }
}
