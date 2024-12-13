namespace IT_Institute_Management.DTO.RequestDTO
{
    using System.ComponentModel.DataAnnotations;

    public class EmailRequestDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; }  

        [Required(ErrorMessage = "Body is required")]
        [StringLength(500, ErrorMessage = "Body cannot exceed 500 characters")]
        public string Body { get; set; }  

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, ErrorMessage = "Subject cannot exceed 100 characters")]
        public string Subject { get; set; }
    }

}
