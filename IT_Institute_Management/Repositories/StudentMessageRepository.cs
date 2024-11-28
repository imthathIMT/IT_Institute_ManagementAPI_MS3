using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
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

        public async Task<IEnumerable<StudentMessage>> GetAllAsync()
        {
            var data = await _context.StudentMessages
                                 .Include(sm => sm.Student)
                                 .ThenInclude(s => s.Address)
                                 .ToListAsync();

            if (data != null)
            {
                return data;
            }
            else
            {
                throw new Exception("Student messages not found");
            }
        }



    }
}
