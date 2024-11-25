using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.DTO.RequestDTO
{
    public class SocialMediaLinksRequestDto
    {
        public string? LinkedIn { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? GitHub { get; set; }
        public string? WhatsApp { get; set; }

        [Required(ErrorMessage = "NIC is required.")]
        [RegularExpression(@"^\d{9}[vxzVXZ]$|^\d{12}$", ErrorMessage = "Invalid NIC format. NIC must either be 9 digits followed by a letter (v/x/z) or 12 digits.")]
        public string StudentNIC { get; set; }
    }
}
