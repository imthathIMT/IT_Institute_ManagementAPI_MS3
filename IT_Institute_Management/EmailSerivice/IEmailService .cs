namespace IT_Institute_Management.EmailSerivice
{
    public interface IEmailService
    {
        Task SendEmail(string receptor, string subject, string body);
    }
}
