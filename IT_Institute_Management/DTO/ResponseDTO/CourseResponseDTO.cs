using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.DTO.ResponseDTO
{
    public class CourseResponseDTO
    {
        [Key]
        public Guid Id { get; set; }
       
        public string CourseName { get; set; }
       
        public string Level { get; set; }
      
        public int Duration { get; set; }
      
        public decimal Fees { get; set; }

        public List<string> ImagePaths { get; set; }

        public string? Description { get; set; }
    }
}
