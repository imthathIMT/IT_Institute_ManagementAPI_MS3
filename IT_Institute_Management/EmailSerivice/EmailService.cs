using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using IT_Institute_Management.EmailSerivice;

namespace IT_Institute_Management.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _senderEmail = "devhubinstitute@gmail.com"; 
        private readonly string _senderPassword = "Imthath2002"; 

        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("DevHub Institute", _senderEmail));
                message.To.Add(new MailboxAddress("", recipientEmail));
                message.Subject = subject;
                message.Body = new TextPart("plain") { Text = body };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_senderEmail, _senderPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                Console.WriteLine($"Email sent to {recipientEmail} successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }

        public async Task SendBulkEmailAsync(string subject, string body, List<string> recipientEmails)
        {
            foreach (var recipientEmail in recipientEmails)
            {
                await SendEmailAsync(recipientEmail, subject, body);
            }
        }
    }
}
