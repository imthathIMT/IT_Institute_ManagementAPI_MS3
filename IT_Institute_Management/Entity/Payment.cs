using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Entity
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment date is required.")]
        public DateTime PaymentDate { get; set; }

        public Enrollment? Enrollment { get; set; } // Navigation property

        public Guid? EnrollmentId { get; set; }  // Foreign key to Enrollment

    }
}
