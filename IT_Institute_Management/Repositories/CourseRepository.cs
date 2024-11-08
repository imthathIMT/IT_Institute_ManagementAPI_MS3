using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace IT_Institute_Management.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly InstituteDbContext _context;

        public CourseRepository(InstituteDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }



        public async Task<Course> GetCourseByIdAsync(Guid id)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task AddCourseAsync(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateCourseAsync(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteCourseAsync(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Course not found.");
            }
        }

        public async Task<bool> CourseExistsAsync(Guid id)
        {
            return await _context.Courses.AnyAsync(c => c.Id == id);
        }
    }
}
