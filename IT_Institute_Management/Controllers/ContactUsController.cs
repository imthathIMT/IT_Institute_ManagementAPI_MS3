using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsService _contactUsService;

        public ContactUsController(IContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }

        [HttpGet]
        [Authorize(Roles = "MasterAdmin, Admin")]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await _contactUsService.GetAllAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "MasterAdmin, Admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var contact = await _contactUsService.GetByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ContactUsRequestDto contactUsDto)
        {
            try
            {
                await _contactUsService.AddAsync(contactUsDto);
                return Ok("Enquiry is posted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ContactUsRequestDto contactUsDto)
        {
            await _contactUsService.UpdateAsync(id, contactUsDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MasterAdmin, Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _contactUsService.DeleteAsync(id);
                return Ok("Successfuly deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPost("send-email")]
        [Authorize(Roles = "MasterAdmin, Admin")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequestDTO emailRequestDto)
        {
            try
            {
                // Validate the emailRequestDto here if needed
                if (emailRequestDto == null)
                {
                    return BadRequest("Email request data is missing.");
                }

                var result = await _contactUsService.ReplyMail(emailRequestDto);

                // If sending is successful, return a success message
                if (result == "Email sent successfully.")
                {
                    return Ok(result);
                }

                // Return validation or specific error messages
                return BadRequest(result);
            }
            catch (ArgumentNullException ex)
            {
                // Handle null argument errors
                return BadRequest($"Error: Missing required information. {ex.Message}");
            }
            catch (FormatException ex)
            {
                // Handle format issues (e.g., invalid email format)
                return BadRequest($"Error: Invalid email format. {ex.Message}");
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

    }
}
