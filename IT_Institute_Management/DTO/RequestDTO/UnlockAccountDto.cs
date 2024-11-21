using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.DTO.RequestDTO
{
    public class UnlockAccountDto
    {
        [Required(ErrorMessage = "NIC is required.")]
        [RegularExpression(@"^\d{9}[vxzVXZ]$|^\d{12}$", ErrorMessage = "Invalid NIC format.")]
        public string NIC { get; set; }


        [Required(ErrorMessage = "New password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
        public string NewPassword { get; set; }
    }
}
