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


        [HttpPut("{nic}")]
        public async Task<IActionResult> UpdateAdmin(string nic, [FromBody] AdminRequestDto adminDto)
        {
            try
            {
                // Ensure that the NIC in the request URL matches the NIC in the request body
                if (nic != adminDto.NIC)
                {
                    return BadRequest(new { message = "NIC in URL and body must be the same." });
                }

                // Call the service method to update the admin
                await _adminService.UpdateAsync(adminDto);
                return NoContent(); // Return 204 No Content for a successful update
            }
            catch (KeyNotFoundException ex)
            {
                // If admin is not found, return a 404 Not Found
                return NotFound(new { message = ex.Message });
            }
            catch (ApplicationException ex)
            {
                // If there's an error with the application (e.g., database issue), return 500 with the message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Any unexpected error, return 500 with a generic message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while updating the admin." });
            }
        }



        [HttpDelete("{nic}")]
        public async Task<IActionResult> Delete([FromRoute] string nic)
        {
            try
            {
                await _adminService.DeleteAsync(nic);
                return NoContent(); // 204 No Content on successful deletion
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message }); // Return 400 with detailed error
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"An error occurred: {ex.Message}" }); // Return 500 for other errors
            }
        }



    }
}
