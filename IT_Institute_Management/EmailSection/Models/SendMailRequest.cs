namespace IT_Institute_Management.EmailSection.Models
{
    public class SendMailRequest
    {
        public string? NIC { get; set; }
        public string? firstName {  get; set; }
        public string? lastName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        //public EmailTypes? EmailType { get; set; }
        public string? TemplateName { get; set; }
    }

    public enum EmailTypes
    {
        None = 0,
        otp,
        Deactive,
        PaymentNotification,

    }
}
