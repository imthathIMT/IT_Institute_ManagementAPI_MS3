using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(Guid id);
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
    }
}
