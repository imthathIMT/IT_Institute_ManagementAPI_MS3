using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.EmailSection.Models;
using IT_Institute_Management.EmailSection.Repo;

namespace IT_Institute_Management.EmailSection.Service
{
    public class sendmailService(SendMailRepository _sendMailRepository, EmailServiceProvider _emailServiceProvider)
    {
        public async Task<string> Sendmail(SendMailRequest sendMailRequest)
        {
            if (sendMailRequest == null) throw new ArgumentNullException(nameof(sendMailRequest));

            var template = await _sendMailRepository.GetTemplate(sendMailRequest.TemplateName).ConfigureAwait(false);
            if (template == null) throw new Exception("Template not found");

            var bodyGenerated = await EmailBodyGenerate(template.TemplateBody!, sendMailRequest);

            MailModel mailModel = new MailModel
            {
                Subject = template.TemplateSubject ?? string.Empty,
                Body = bodyGenerated ?? string.Empty,
                SenderName = "Sample System",
                To = sendMailRequest.Email ?? throw new Exception("Recipient email address is required")
            };

            await _emailServiceProvider.SendMail(mailModel).ConfigureAwait(false);

            return "email was sent successfully";
        }

        public async Task<string> EmailBodyGenerate(string emailbody, SendMailRequest sendMailRequest)
        {
            var replacements = new Dictionary<string, string?>
            {
                { "{{FirstName}}", sendMailRequest.firstName },
                 {"{{LastName}}", sendMailRequest.lastName },
                  { "{{NICNumber}}", sendMailRequest.NIC },
                   { "{{Password}}", sendMailRequest.Password },
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
