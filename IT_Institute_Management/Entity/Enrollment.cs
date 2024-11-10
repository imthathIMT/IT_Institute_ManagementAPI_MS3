using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Entity
{
    public class Enrollment
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Enrollment date is required.")]
        public DateTime EnrollmentDate { get; set; }

        [Required(ErrorMessage = "Payment plan is required.")]
        public string PaymentPlan { get; set; }  // e.g., Monthly, Full

        public Student Student { get; set; } // Navigation property
        public Course Course { get; set; } // Navigation property

        [Required(ErrorMessage = "Student NIC is required.")]
        public string StudentNIC { get; set; }  // Foreign key to Student

        [Required(ErrorMessage = "Course ID is required.")]
        public Guid CourseId { get; set; }  // Foreign key to Course


        public ICollection<Payment>? payments { get; set; }
    }
}
