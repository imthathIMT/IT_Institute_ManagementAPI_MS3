using System.Net.Mail;
using System.Net;
using IT_Institute_Management.EmailSerivice;
using IT_Institute_Management.Entity;
using Microsoft.Data.SqlClient;
using IT_Institute_Management.DTO.RequestDTO;

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

        public void SendRegistraionMail(string recipientEmail, StudentRequestDto student)
        {
            Task.Run(async () =>
            {
                await SendEmailBackground(recipientEmail, student);
            });

        }

        public async Task SendEmailBackground(string recipientEmail, StudentRequestDto student)
        {
            var templateService = new EmailTemplateService();
            var templateBody = await templateService.GetTemplateByNameAsync("RegistrationWelcome");

            if (string.IsNullOrEmpty(templateBody))
            {
                throw new InvalidOperationException("Email template not found.");
            }

            var populatedBody = templateService.PopulateTemplate(templateBody, student);

            // Send the email in background
            await Task.Run(() => SendEmail(recipientEmail, "Student Registration", populatedBody));
        }


    }


    public class EmailTemplateService
    {
        private readonly string _connectionString = "Server =(localdb)\\MSSQLLocalDB; Database=DevHub; Trusted_Connection=True; TrustServerCertificate=True;";

        public async Task<string> GetTemplateByNameAsync(string templateName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT TemplateBody FROM EmailTemplates WHERE TemplateName = @TemplateName";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TemplateName", templateName);
                    var result = await command.ExecuteScalarAsync();
                    return result?.ToString();
                }
            }
        }

        public string PopulateTemplate(string template, StudentRequestDto student)
        {
            return template
                .Replace("{{FirstName}}", student.FirstName)
                .Replace("{{LastName}}", student.LastName)
                .Replace("{{NICNumber}}", student.NIC)
                .Replace("{{Password}}", student.Password);  // Exclude password if you wish
        }
    }

}
