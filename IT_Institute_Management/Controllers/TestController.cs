using IT_Institute_Management.EmailSerivice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public TestController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        [Authorize(Roles = "MasterAdmin")]
        public async Task<IActionResult> Email(string email, string subject, string body)
        {
            try
            {
               await _emailService.SendEmail(email,subject, body);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("test")]
        [Authorize(Roles = "MasterAdmin")]
        public async Task<IActionResult> SampleTest()
        {
           return Ok("It's work bro");
        }
    }
}
