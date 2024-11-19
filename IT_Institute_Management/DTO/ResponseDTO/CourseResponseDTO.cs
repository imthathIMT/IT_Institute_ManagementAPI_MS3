using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.DTO.ResponseDTO
{
    public class CourseResponseDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Course name is required.")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Level is required.")]
        public string Level { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0.")]
        public int Duration { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Fees must be greater than 0.")]
        public decimal Fees { get; set; }

        [Required(ErrorMessage = "Image path is required.")]
        [MaxLength(255, ErrorMessage = "Image path cannot exceed 255 characters.")]
        public List<string> ImagePaths { get; set; }
    }
}
