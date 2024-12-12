using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.EmailSection.Models;
using IT_Institute_Management.EmailSection.Repo;
using IT_Institute_Management.IRepositories;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace IT_Institute_Management.EmailSection.Service
{
    public class sendmailService(SendMailRepository _sendMailRepository, EmailServiceProvider _emailServiceProvider, IStudentRepository _studentRepository, IConfiguration _Configuration)
    {
        private object configuration;
         

        public void sendmail(SendMailRequest sendMailRequest)
        {
            Task.Run(async () =>
            {
                await Sendmail(sendMailRequest);
            });
        }


        public void sendBulkMail(SendMailRequest sendMailRequest)
        {
            Task.Run(async () =>
            {
                await SendBulkCourseEmail(sendMailRequest);
            });
        }


        public async Task<string> Sendmail(SendMailRequest sendMailRequest)
        {
            if (sendMailRequest == null) throw new ArgumentNullException(nameof(sendMailRequest));

            var template = await _sendMailRepository.GetTemplate(sendMailRequest.TemplateName!).ConfigureAwait(false);
            if (template == null) throw new Exception("Template not found");

            var bodyGenerated = await EmailBodyGenerate(template.TemplateBody!, sendMailRequest);

            MailModel mailModel = new MailModel
            {
                Subject = template.TemplateSubject ?? string.Empty,
                Body = bodyGenerated ?? string.Empty,
                SenderName = "DevHub Institute",
                To = sendMailRequest.Email ?? throw new Exception("Recipient email address is required")
            };

            await _emailServiceProvider.SendMail(mailModel).ConfigureAwait(false);

            return "email was sent successfully";
        }


        // Bulk email sending function
        public async Task<string> SendBulkCourseEmail(SendMailRequest sendMailRequest)
        {
            if (sendMailRequest == null) throw new ArgumentNullException(nameof(sendMailRequest));

           

            // Get all students (you can customize this query as needed)
            var students = await _studentRepository.GetAllAsync(); // Fetch students from the database

            if (students == null || students.Count == 0)
            {
                return "No students found to send emails to.";
            }

            // Loop through each student and send an email
            foreach (var student in students)
            {
                var template = await _sendMailRepository.GetTemplate(sendMailRequest.TemplateName!).ConfigureAwait(false);
                if (template == null) throw new Exception("Template not found"); 

                // Generate the email content for each student
                var emailBody = await EmailBodyGenerate(template.TemplateBody!, sendMailRequest);

                var mailModel = new MailModel
                {
                    Subject = template.TemplateSubject ?? string.Empty,
                    Body = emailBody,
                    SenderName = "DevHub Institute",
                    To = student.Email // Assuming students have an Email field
                };

                // Send email to the student
                await _emailServiceProvider.SendMail(mailModel);
            }

            return $"{students.Count} emails were sent successfully.";
        }


        public async Task<string> EmailBodyGenerate(string emailbody, SendMailRequest sendMailRequest)
        {
            var fees = sendMailRequest.Fees + ".00 LKR";
            var duration = sendMailRequest.Duration + "Months";
            var EnrollmentDate = sendMailRequest.StartDate.ToString();
            var amountPaid = sendMailRequest.AmountPaid + ".00 LKR";
            var replacements = new Dictionary<string, string?>
            {
                { "{{FirstName}}", sendMailRequest.FirstName },
                {"{{LastName}}", sendMailRequest.LastName },
                { "{{NICNumber}}", sendMailRequest.NIC },
                { "{{Password}}", sendMailRequest.Password ?? "Not provided"  },
                { "{{CourseName}}",sendMailRequest.CourseName },
                { "{{Fees}}",fees },
                { "{{Duration}}",duration },
                { "{{Level}}",sendMailRequest.Level },
                { "{{StartDate}}",EnrollmentDate },
                { "{{Payment plan}}",sendMailRequest.PaymentPlan },
                { "{{AmountPaid}}", amountPaid },
            };

            foreach (var replacement in replacements)
            {
                if (!string.IsNullOrEmpty(replacement.Value))
                {
                    emailbody = emailbody.Replace(replacement.Key, replacement.Value, StringComparison.OrdinalIgnoreCase);
                }
            }

            return emailbody;
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
            var email = _Configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
            var password = _Configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
            var host = _Configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = _Configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

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
