namespace IT_Institute_Management.EmailSection.Models
{
    public class SendMailRequest
    {
        // Welcome Details
        public string? NIC { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        // Template Name to identify the email type
        public string? TemplateName { get; set; }

        // New course offering
        public string? CourseName { get; set; }
        public int? Duration { get; set; }
        public decimal? Fees { get; set; }
        public string? Level { get; set; }

        //// Payment Reminder Email
        //public string? AmountDue { get; set; }
        //public string? DueDate { get; set; }

        //// Enrollment Email
        //public string? StartDate { get; set; }
        //public string? PaymentPlan { get; set; }


        //// Payment Success Email
        //public string? AmountPaid { get; set; }
        //public string? PaymentMethod { get; set; }

    }

}
