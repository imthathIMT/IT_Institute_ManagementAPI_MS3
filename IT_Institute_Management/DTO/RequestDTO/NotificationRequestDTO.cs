using System.ComponentModel.DataAnnotations;
namespace IT_Institute_Management.DTO.RequestDTO
{
    public class NotificationRequestDTO
    {
        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; }


        public string? StudentNIC { get; set; }
    }
}
