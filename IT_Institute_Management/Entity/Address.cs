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

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        // New ZipCode property with validation
        [Required(ErrorMessage = "Zip code is required.")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid zip code format. Use 12345 or 12345-6789.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }

        // Foreign Key
        public string? StudentNIC { get; set; }

        // Navigation property
        public Student? Student { get; set; }
    }

}
