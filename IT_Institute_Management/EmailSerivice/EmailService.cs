using System.Net.Mail;
using System.Net;
using IT_Institute_Management.EmailSerivice;

namespace IT_Institute_Management.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendEmailInBackground(string receptor, string subject, string body)
        {
            // Run the SendEmail method in a background task
            Task.Run(async () =>
            {
                await SendEmail(receptor, subject, body);
            });
        }

        public async Task SendEmail(string receptor, string subject, string body)
        {
            var email = configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
            var password = configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
            var host = configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            // Use the 'using' statement to ensure proper disposal of resources
            using (var smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email, password)
            })
            using (var message = new MailMessage(email, receptor, subject, body))
            {
                // Send the email asynchronously
                await smtpClient.SendMailAsync(message);
            }
        }

    }
}
