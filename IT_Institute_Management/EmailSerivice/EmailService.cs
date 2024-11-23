using IT_Institute_Management.IRepositories;
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
        private readonly string _senderEmail = "devhubinstitute@gmail.com"; // Sender email
        private readonly string _senderPassword = "Imthath2002";

        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            try
            {
                using (var message = new MailMessage())
                using (var client = new SmtpClient(_smtpServer))
                {
                    message.From = new MailAddress(_senderEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = false;
                    message.To.Add(new MailAddress(recipientEmail));

                    client.EnableSsl = true;
                    client.Port = _smtpPort;
                    client.Credentials = new NetworkCredential(_senderEmail, _senderPassword);

                    await client.SendMailAsync(message);
                }

                Console.WriteLine($"Email sent to {recipientEmail} successfully.");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log to file or an external system)
                Console.WriteLine($"Error sending email to {recipientEmail}: {ex.Message}");
            }
        }

        public async Task SendBulkEmailAsync(string subject, string body, List<string> recipientEmails)
        {
            try
            {
                if (recipientEmails == null || recipientEmails.Count == 0)
                {
                    Console.WriteLine("No email addresses provided.");
                    return;
                }

                using (var message = new MailMessage())
                using (var client = new SmtpClient(_smtpServer))
                {
                    message.From = new MailAddress(_senderEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = false;

                    // Add each email address to the recipient list
                    foreach (var email in recipientEmails)
                    {
                        message.To.Add(new MailAddress(email));
                    }

                    client.EnableSsl = true;
                    client.Port = _smtpPort;
                    client.Credentials = new NetworkCredential(_senderEmail, _senderPassword);

                    await client.SendMailAsync(message);
                }

                Console.WriteLine("Bulk email sent successfully.");
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.WriteLine($"Error sending bulk email: {ex.Message}");
            }
        }
    }
}
