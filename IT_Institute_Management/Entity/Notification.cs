using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Entity
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [MaxLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        // Foreign key to Student
        public string? StudentNIC { get; set; }
        public Student? Student { get; set; }
    }
}
