using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Entity
{

    public class Admin
    {


        [Key]
        [Required(ErrorMessage = "NIC is required.")]
        [RegularExpression(@"^\d{9}[vxzVXZ]$|^\d{12}$", ErrorMessage = "Invalid NIC format. NIC must either be 9 digits followed by a letter (v/x/z) or 12 digits.")]
        public string NIC { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(15, ErrorMessage = "Phone number must not exceed 15 digits.")]
        public string Phone { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
