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

        [Required(ErrorMessage = "Full amount is required.")]
        public decimal FullAmount { get; set; }

        [Required(ErrorMessage = "Due amount is required.")]
        public decimal DueAmount { get; set; }

        public Enrollment Enrollment { get; set; } // Navigation property

        [Required(ErrorMessage = "Enrollment ID is required.")]
        public Guid EnrollmentId { get; set; }  // Foreign key to Enrollment

    }
}
