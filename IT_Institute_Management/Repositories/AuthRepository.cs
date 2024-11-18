using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace IT_Institute_Management.Repositories
{
    public class AuthRepository: IAuthRepository
    {
        private readonly InstituteDbContext _context;

        public AuthRepository(InstituteDbContext context)
        {
            _context = context;
        }


        public async Task<User> GetLoginUser(string nic)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.NIC == nic);
        }
    }
}
