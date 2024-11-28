using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Entity
{
    public class SocialMediaLinks
    {
        [Key]
        public Guid Id { get; set; } // Primary Key

        public string? LinkedIn { get; set; } // LinkedIn link

        public string? Instagram { get; set; } // Instagram link

        public string? Facebook { get; set; } // Facebook link

        public string? GitHub { get; set; } // GitHub link

        public string? WhatsApp { get; set; } // WhatsApp link

        // Foreign key relationship to User table
        [ForeignKey("User")]
        public string? StudentNIC { get; set; } // Foreign key field

        // Navigation property to the User entity
        public virtual Student? Student { get; set; }
    }
}
