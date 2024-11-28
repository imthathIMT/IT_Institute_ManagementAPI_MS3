using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentMessageController : ControllerBase
    {
        private readonly IStudentMessageService _service;

        public StudentMessageController(IStudentMessageService studentMessageService)
        {
            _service = studentMessageService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentMessageResponseDto>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllMessagesAsync();
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
