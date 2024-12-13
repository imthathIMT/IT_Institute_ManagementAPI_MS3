using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Entity
{
    public class SocialMediaLinks
    {
        [Key]
        public Guid Id { get; set; }

        public string? LinkedIn { get; set; } 

        public string? Instagram { get; set; } 

        public string? Facebook { get; set; } 

        public string? GitHub { get; set; } 

        public string? WhatsApp { get; set; } 

      
        [ForeignKey("User")]
        public string? StudentNIC { get; set; } 

        
        public virtual Student? Student { get; set; }
    }
}
