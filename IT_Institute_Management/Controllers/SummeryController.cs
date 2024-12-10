using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummeryController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        // Inject services through constructor
        public SummeryController(IStudentService studentService, ICourseService courseService)
        {
            _studentService = studentService;
            _courseService = courseService;
        }

        // Get the summary (Total Students and Total Courses)
        [HttpGet("summary")]
        public async Task<ActionResult<SummaryResponseDto>> GetSummary()
        {
            // Fetch students and courses
            var students = await _studentService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();

            // Create summary response DTO
            var summary = new SummaryResponseDto
            {
                TotalStudents = students.Count,
                TotalCourses = courses.Count()
            };

            return Ok(summary);
        }
    }
}
