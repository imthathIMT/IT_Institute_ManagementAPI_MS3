using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Entity
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Course name is required.")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Level is required.")]
        public string Level { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        public int Duration { get; set; }  // Duration in hours or weeks

        [Required(ErrorMessage = "Fees are required.")]
        public decimal Fees { get; set; }

        [Required(ErrorMessage = "Image path is required.")]
        public string ImagePath { get; set; }
    }
}
