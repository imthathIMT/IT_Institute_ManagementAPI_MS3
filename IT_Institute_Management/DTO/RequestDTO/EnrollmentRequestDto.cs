using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.DTO.RequestDTO
{
    public class EnrollmentRequestDto
    {
        [Required(ErrorMessage = "Payment plan is required.")]
        public string PaymentPlan { get; set; }

        [Required(ErrorMessage = "Student NIC is required.")]
        public string StudentNIC { get; set; }  // Foreign key to Student

        [Required(ErrorMessage = "Course ID is required.")]
        public Guid CourseId { get; set; }  // Foreign key to Course
    }
}
