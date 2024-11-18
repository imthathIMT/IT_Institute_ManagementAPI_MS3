using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentRequestDto enrollmentRequest)
        {
            try
            {
                var enrollment = await _enrollmentService.CreateEnrollmentAsync(enrollmentRequest);
                return Ok(new { EnrollmentId = enrollment.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetAllEnrollments()
        {
            try
            {
                var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



  
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompletionStatus(Guid id)
        {
            try
            {
                var updatedEnrollment = await _enrollmentService.UpdateEnrollmentCompletionStatus(id);
                return Ok(new { IsComplete = updatedEnrollment.IsComplete });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateEnrollmentData(Guid id, [FromBody] EnrollmentRequestDto enrollmentRequest)
        {
            try
            {
                var updatedEnrollment = await _enrollmentService.UpdateEnrollmentDataAsync(id, enrollmentRequest);
                return Ok(updatedEnrollment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnrollmentById(Guid id)
        {
            try
            {
                var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
                if (enrollment == null)
                {
                    return NotFound(new { message = "Enrollment not found." });
                }
                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("nic/{nic}")]
        public async Task<IActionResult> GetEnrollmentsByNIC(string nic)
        {
            try
            {
                var enrollments = await _enrollmentService.GetEnrollmentsByNICAsync(nic);
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEnrollmentById(Guid id, [FromQuery] bool forceDelete = false)
        {
            try
            {
                var enrollment = await _enrollmentService.DeleteEnrollmentByIdAsync(id, forceDelete);
                return Ok(new { Message = "Enrollment deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
