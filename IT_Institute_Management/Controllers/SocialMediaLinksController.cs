using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediaLinksController : ControllerBase
    {
        private readonly ISocialMediaLinksService _service;

        public SocialMediaLinksController(ISocialMediaLinksService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework like Serilog, NLog, or log to a file/database)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{nic}")]
        public async Task<IActionResult> GetByNIC(string nic)
        {
            try
            {
                var result = await _service.GetByNICAsync(nic);
                if (result == null)
                {
                    return NotFound(new { Message = $"No Social Media Links found for NIC: {nic}" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SocialMediaLinksRequestDto requestDto)
        {
            try
            {
                var result = await _service.CreateAsync(requestDto);
                return CreatedAtAction(nameof(GetByNIC), new { nic = result.StudentNIC }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] SocialMediaLinksRequestDto requestDto)
        {
            try
            {
                var result = await _service.UpdateAsync(id, requestDto);
                if (result == null)
                {
                    return NotFound(new { Message = $"No Social Media Links found for ID: {id}" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{nic}")]
        public async Task<IActionResult> Delete(string nic)
        {
            try
            {
                var result = await _service.DeleteAsync(nic);

                // Check if deletion was successful
                if (!result)
                {
                    return NotFound(new { Message = $"No Social Media Links found for ID: {nic}" });
                }

                return Ok(new { Message = "Social Media Links deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to delete Social Media Links.", Details = ex.Message });
            }
        }

    }
}

