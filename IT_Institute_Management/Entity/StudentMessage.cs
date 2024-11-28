using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Entity
{
    public class StudentMessage
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        // Student NIC as foreign key
        [ForeignKey("Student")]
        public string? StudentNIC { get; set; }

        // Navigation property to the Student entity
        public Student? Student { get; set; }
    }
}
