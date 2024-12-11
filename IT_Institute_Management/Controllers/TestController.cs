using IT_Institute_Management.EmailSection.Models;
using IT_Institute_Management.EmailSection.Service;
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
        private readonly sendmailService _sendmailService;

        public TestController(IEmailService emailService, sendmailService sendmailService)
        {
            _emailService = emailService;
            _sendmailService = sendmailService;
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


        [HttpPost("Send-Mail")]
        public async Task<IActionResult> Sendmail(SendMailRequest sendMailRequest)
        {
            var res = await _sendmailService.Sendmail(sendMailRequest).ConfigureAwait(false);
            return Ok(res);
        }
    }
}
