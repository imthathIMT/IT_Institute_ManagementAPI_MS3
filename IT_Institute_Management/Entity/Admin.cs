using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Entity
{
    public class Admin
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string NIC { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
    }
}
