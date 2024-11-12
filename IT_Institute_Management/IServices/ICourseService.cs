using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface ICourseService
    {
        // Get all courses
        Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync();

        // Get a course by ID
        Task<CourseResponseDTO> GetCourseByIdAsync(Guid id);

        // Create a course with multiple images (images are provided as IFormFile)
        Task CreateCourseAsync(CourseRequestDTO courseRequest, List<IFormFile> images);

        // Update an existing course with multiple images (images are provided as IFormFile)
        Task UpdateCourseAsync(Guid id, CourseRequestDTO courseRequest, List<IFormFile> images);

        // Delete a course by ID
        Task DeleteCourseAsync(Guid id);
    }
}
