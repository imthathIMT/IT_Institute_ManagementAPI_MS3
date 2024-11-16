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
        public string PaymentPlan { get; set; } 

        public bool IsComplete { get; set; } = false;

        public Student Student { get; set; } 
        public Course Course { get; set; } 

        [Required(ErrorMessage = "Student NIC is required.")]
        public string StudentNIC { get; set; } 

        [Required(ErrorMessage = "Course ID is required.")]
        public Guid CourseId { get; set; }  

        public ICollection<Payment>? payments { get; set; }
    }
}
