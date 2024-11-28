using IT_Institute_Management.EmailSerivice;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        // Injecting EmailService into the controller
        public TestMailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // Endpoint to send a single email
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(string email,string body,string subject)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
            {
                return BadRequest("Email, Subject, and Body are required.");
            }

            await _emailService.SendEmail(email, subject,body);
            return Ok("Email sent successfully.");
        }

        // Endpoint to send bulk emails
        //[HttpPost("send-bulk")]
        //public async Task<IActionResult> SendBulkEmail(List<string> Emails, string body, string subject)
        //{
        //    if (Emails == null || Emails.Count == 0 || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
        //    {
        //        return BadRequest("Emails, Subject, and Body are required.");
        //    }

        //    await _emailService.SendBulkEmailAsync(subject, body,Emails);
        //    return Ok("Bulk email sent successfully.");
        //}
    }
}
