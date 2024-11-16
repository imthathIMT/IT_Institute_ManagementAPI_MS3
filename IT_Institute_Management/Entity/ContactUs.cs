using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Entity
{
    public class ContactUs
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; }

        public DateTime Date { get; set; } = DateTime.Now; 
    }
}
