using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.EmailSection.Models
{
    public class EmailTemplate
    {
        [Key]
        public int TemplateId { get; set; }
        public string? TemplateName { get; set; }
        public string? TemplateSubject { get; set; }
        public string? TemplateBody { get; set; }
    }
}
