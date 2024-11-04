namespace IT_Institute_Management.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Address
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Address line 1 is required.")]
        public string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "Province is required.")]
        public string Province { get; set; }

        [Required(ErrorMessage = "District is required.")]
        public string District { get; set; }

        [Required(ErrorMessage = "Student NIC is required.")]

        public Student Student { get; set; }
        public Guid StudentNIC { get; set; }  // Foreign key to Student
    }

}
