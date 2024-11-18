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

            
            using (var client = new SmtpClient("smtp.gmail.com"))
            {
               
                client.EnableSsl = true;
                client.Port = 587;

                
                client.Credentials = new NetworkCredential("ut01635tic@gmail.com", "yourpassword");

                try
                {
                    await client.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                   
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }
    }

}
