using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.DTO.RequestDTO
{
    public class CourseRequestDTO
    {
        [Required(ErrorMessage = "Course name is required.")]
        [MaxLength(100, ErrorMessage = "Course name cannot exceed 100 characters.")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Level is required.")]
        [MaxLength(50, ErrorMessage = "Level cannot exceed 50 characters.")]
        public string Level { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0.")]
        public int Duration { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Fees must be greater than 0.")]
        public decimal Fees { get; set; }


        [Required(ErrorMessage = "At least one image is required.")]
        public List<IFormFile> Images { get; set; }

        public string? Description { get; set; }
    }
}
