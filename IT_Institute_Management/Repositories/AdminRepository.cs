using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace IT_Institute_Management.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly InstituteDbContext _instituteDbContext;
        public AdminRepository(InstituteDbContext context) { _instituteDbContext = context; }
        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            return await _instituteDbContext.Admins.ToListAsync();
        }
        public async Task<Admin> GetByIdAsync(string nic)
        {
            return await _instituteDbContext.Admins.FindAsync(nic);
        }
        public async Task AddAsync(Admin admin)
        {
            await _instituteDbContext.Admins.AddAsync(admin);
            await _instituteDbContext.SaveChangesAsync();
        }


        public async Task UpdateAsync(Admin admin)
        {
            _instituteDbContext.Admins.Update(admin);
            await _instituteDbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(string nic)
        {
            var admin = await _instituteDbContext.Admins.FindAsync(nic);
            if (admin != null)
            {
                _instituteDbContext.Admins.Remove(admin);
                await _instituteDbContext.SaveChangesAsync();
            }
        }



    }
}
