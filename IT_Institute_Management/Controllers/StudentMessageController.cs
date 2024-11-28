using IT_Institute_Management.DTO.RequestDTO;
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

        [HttpGet("by-student/{studentNIC}")]
        public async Task<ActionResult<IEnumerable<StudentMessageResponseDto>>> GetByStudentNIC(string studentNIC)
        {
            try
            {
                var result = await _service.GetMessagesByStudentNICAsync(studentNIC);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<StudentMessageResponseDto>> Post([FromBody] StudentMessageRequestDto requestDto)
        {
            try
            {
                var result = await _service.AddMessageAsync(requestDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Return a BadRequest response with the exception message
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _service.DeleteMessageAsync(id);
                if (success)
                {
                    // Return a success message with a 200 OK response
                    return Ok(new { message = "Delete successful" });
                }
                else
                {
                    // Return a 404 Not Found with a message if the entity was not found
                    return NotFound(new { message = "Message not found for the given ID." });
                }
            }
            catch (Exception ex)
            {
                // Return a 400 Bad Request with the error message
                return BadRequest(new { message = "Error deleting the message: " + ex.Message });
            }
        }


    }
}
