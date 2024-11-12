using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface ICourseService
    {
      Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync();
      Task<CourseResponseDTO> GetCourseByIdAsync(Guid id);
      Task CreateCourseAsync(CourseRequestDTO courseRequest, List<IFormFile> images);     
      Task UpdateCourseAsync(Guid id, CourseRequestDTO courseRequest, List<IFormFile> images);
      Task DeleteCourseAsync(Guid id);
    }
}
