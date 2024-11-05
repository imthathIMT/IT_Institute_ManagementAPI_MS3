using System.Net;
using System.Net.Mail;

namespace IT_Institute_Management.EmailSerivice
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress("ut01635tic@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            message.To.Add(new MailAddress(email));

            // SMTP client configuration for Gmail
            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                // Enable SSL and set port for TLS
                client.EnableSsl = true;
                client.Port = 587;

                // Set up authentication using your Gmail credentials
                client.Credentials = new NetworkCredential("ut01635tic@gmail.com", "yourpassword");

                try
                {
                    await client.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., invalid credentials, network issues)
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }
    }

}
