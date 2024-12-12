using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.EmailSection.Models;
using IT_Institute_Management.EmailSection.Repo;
using IT_Institute_Management.IRepositories;

namespace IT_Institute_Management.EmailSection.Service
{
    public class sendmailService(SendMailRepository _sendMailRepository, EmailServiceProvider _emailServiceProvider, IStudentRepository _studentRepository)
    {


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

       
    }
}
