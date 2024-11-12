using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.DTO.RequestDTO
{
    public class AdminRequestDto
    {
        [Required(ErrorMessage = "NIC is required.")]
        [RegularExpression(@"^\d{9}[vxzVXZ]$|^\d{12}$", ErrorMessage = "Invalid NIC format.")]
        public string NIC { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(15, ErrorMessage = "Phone number must not exceed 15 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least 1 uppercase letter, 1 lowercase letter, and 1 special character.")]
        public string Password { get; set; }

       
    }
}
