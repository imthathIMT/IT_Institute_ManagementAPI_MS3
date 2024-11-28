using IT_Institute_Management.Database;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace IT_Institute_Management.Repositories
{
    public class StudentMessageRepository : IStudentMessageRepository
    {
        private readonly InstituteDbContext _context;

        public StudentMessageRepository(InstituteDbContext context)
        {
            _context = context;
        }

    }
}
