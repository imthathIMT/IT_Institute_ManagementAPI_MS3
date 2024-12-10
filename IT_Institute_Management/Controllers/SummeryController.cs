using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.DTO.ResponseDTO.SummeryDTO;
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
        private readonly IEnrollmentService _enrollmentService;

        // Inject services through constructor
        public SummeryController(IStudentService studentService, ICourseService courseService, IEnrollmentService enrollmentService)
        {
            _studentService = studentService;
            _courseService = courseService;
            _enrollmentService = enrollmentService;
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

        // Get Enrollment Summary (Total, Complete, and Reading Enrollments)
        [HttpGet("enrollment-summary")]
        public async Task<ActionResult<EnrollmentSummaryResponseDto>> GetEnrollmentSummary()
        {
            // Fetch all enrollments
            var allEnrollments = await _enrollmentService.GetAllEnrollmentsAsync();

            // Fetch complete enrollments
            var completeEnrollments = await _enrollmentService.GetEnrollmentsByCompletionStatusAsync(true);

            // Fetch reading enrollments
            var readingEnrollments = await _enrollmentService.GetEnrollmentsByCompletionStatusAsync(false);

            // Create the enrollment summary response DTO
            var summary = new EnrollmentSummaryResponseDto
            {
                TotalEnrollments = allEnrollments.Count(),
                CompleteEnrollments = completeEnrollments.Count(),
                ReadingEnrollments = readingEnrollments.Count()
            };

            return Ok(summary);
        }
    }
}
