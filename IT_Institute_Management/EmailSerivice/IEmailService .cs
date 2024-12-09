using IT_Institute_Management.DTO.RequestDTO;

namespace IT_Institute_Management.EmailSerivice
{
    public interface IEmailService
    {
        Task SendEmail(string receptor, string subject, string body);
        void SendEmailInBackground(string receptor, string subject, string body);
        void SendRegistraionMail(string recipientEmail, StudentRequestDto student);
    }
}
