using IT_Institute_Management.Database;
using IT_Institute_Management.IRepositories;

namespace IT_Institute_Management.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InstituteDbContext _context;

        public UserRepository(InstituteDbContext context)
        {
            _context = context;
        }
    }
}
