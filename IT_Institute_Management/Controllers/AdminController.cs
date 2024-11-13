using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService) {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var admins = await _adminService.GetAllAsync();
                return Ok(admins);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpGet("{nic}")]
        public async Task<IActionResult> GetById(string nic)
        {
            try
            {
                var admin = await _adminService.GetByIdAsync(nic);
                return Ok(admin);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AdminRequestDto adminDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // If model validation fails, return 400 with validation errors
            }

            try
            {
                await _adminService.AddAsync(adminDto);
                return CreatedAtAction(nameof(GetById), new { nic = adminDto.NIC }, adminDto);  // Return 201 Created if successful
            }
            catch (ApplicationException ex)
            {
                // Return 400 Bad Request with the error message for application-level issues
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                // Return 500 Internal Server Error if it's a database-related issue
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Database error: {ex.Message}" });
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error for unexpected issues
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Unexpected error: {ex.Message}" });
            }
        }



        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AdminRequestDto adminDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _adminService.UpdateAsync(adminDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpDelete("{nic}")]
        public async Task<IActionResult> Delete(string nic)
        {
            try
            {
                await _adminService.DeleteAsync(nic);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


    }
}
