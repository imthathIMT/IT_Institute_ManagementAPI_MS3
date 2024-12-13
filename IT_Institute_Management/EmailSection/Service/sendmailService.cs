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


        
        public async Task<string> SendBulkCourseEmail(SendMailRequest sendMailRequest)
        {
            if (sendMailRequest == null) throw new ArgumentNullException(nameof(sendMailRequest));

           

          
            var students = await _studentRepository.GetAllAsync(); 

            if (students == null || students.Count == 0)
            {
                return "No students found to send emails to.";
            }

           
            foreach (var student in students)
            {
                var template = await _sendMailRepository.GetTemplate(sendMailRequest.TemplateName!).ConfigureAwait(false);
                if (template == null) throw new Exception("Template not found"); 

               
                var emailBody = await EmailBodyGenerate(template.TemplateBody!, sendMailRequest);

                var mailModel = new MailModel
                {
                    Subject = template.TemplateSubject ?? string.Empty,
                    Body = emailBody,
                    SenderName = "DevHub Institute",
                    To = student.Email 
                };

            
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
    }
}
