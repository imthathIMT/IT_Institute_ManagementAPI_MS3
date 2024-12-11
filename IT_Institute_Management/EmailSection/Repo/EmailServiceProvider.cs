using IT_Institute_Management.EmailSection.Models;
using MimeKit;

namespace IT_Institute_Management.EmailSection.Repo
{
    public class EmailServiceProvider
    {
        private readonly EmailConfig _config;

        public EmailServiceProvider(EmailConfig config)
        {
            _config = config;
        }

        public async Task SendMail(MailModel mailModel)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_config.FromName, _config.EMAIL));
            message.To.Add(new MailboxAddress("", mailModel.To));
            message.Subject = mailModel.Subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = mailModel.Body,
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect(_config.HOST, _config.PORT, true);
            client.Authenticate(_config.Username, _config.PASSWORD);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
