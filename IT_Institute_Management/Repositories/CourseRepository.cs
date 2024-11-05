using IT_Institute_Management.Database;
using IT_Institute_Management.IRepositories;

namespace IT_Institute_Management.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly InstituteDbContext _context;

        public CourseRepository(InstituteDbContext context)
        {
            _context = context;
        }
    }
}
