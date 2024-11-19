using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IT_Institute_Management.EmailSerivice
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;


        private readonly string Email = "devhubinstitute@gmail.com";
        private readonly string password = "Imthath2002";

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress("devhubinstitute@gmail.com");
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = false;
                    message.To.Add(new MailAddress(email));

                    using (var client = new SmtpClient(_smtpServer))
                    {
                        client.EnableSsl = true;
                        client.Port = _smtpPort;
                        client.Credentials = new NetworkCredential(
                            Environment.GetEnvironmentVariable(Email),
                            Environment.GetEnvironmentVariable(password));

                        await client.SendMailAsync(message);
                    }
                }

                Console.WriteLine($"Email sent to {email} successfully.");
            }
            catch (Exception ex)
            {
                // Log or handle the exception (you could log to a file or external system)
                Console.WriteLine($"Error sending email to {email}: {ex.Message}");
            }
        }

        public async Task SendBulkEmailAsync(List<string> emails, string subject, string body)
        {
            try
            {
                if (emails == null || emails.Count == 0)
                {
                    Console.WriteLine("No email addresses provided.");
                    return;
                }

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress("devhubinstitute@gmail.com");
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = false;

                    // Add recipients to the "To" list
                    foreach (var email in emails)
                    {
                        message.To.Add(new MailAddress(email));
                    }

                    using (var client = new SmtpClient(_smtpServer))
                    {
                        client.EnableSsl = true;
                        client.Port = _smtpPort;
                        client.Credentials = new NetworkCredential(
                            Environment.GetEnvironmentVariable(Email),
                            Environment.GetEnvironmentVariable(password));

                        await client.SendMailAsync(message);
                    }
                }

                Console.WriteLine("Bulk email sent successfully.");
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error sending bulk email: {ex.Message}");
            }
        }
    }
}
