using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Entity
{
    public class Announcement
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Body is required.")]
        public string Body { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }
    }
}
