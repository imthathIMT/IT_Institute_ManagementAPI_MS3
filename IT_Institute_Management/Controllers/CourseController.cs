using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            try
            {
                var course = await _courseService.GetCourseByIdAsync(id);
                return Ok(course);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Course not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
