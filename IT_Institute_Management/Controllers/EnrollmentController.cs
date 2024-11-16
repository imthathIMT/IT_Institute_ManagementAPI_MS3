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
                return BadRequest(ex.Message);
            }
        }

        
        [HttpDelete("delete/{nic}")]
        public async Task<IActionResult> DeleteEnrollmentByNIC(string nic, [FromQuery] bool forceDelete = false)
        {
            try
            {
                var enrollment = await _enrollmentService.DeleteEnrollmentByNICAsync(nic, forceDelete);
                return Ok(new { Message = "Enrollment deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }
        }
    }
}
