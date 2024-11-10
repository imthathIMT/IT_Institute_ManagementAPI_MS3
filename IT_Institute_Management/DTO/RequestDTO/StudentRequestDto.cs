using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.DTO.RequestDTO
{
    public class StudentRequestDto
    {
        [Required(ErrorMessage = "NIC is required.")]
        [RegularExpression(@"^(?!.*[^0-9VXZ]).{9}$|^(?!.*[^0-9]).{12}$",
            ErrorMessage = "NIC must be either 9 digits followed by V/Z/X or 12 digits.")]
        public string NIC { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?(\d{1,4})?[\s\-]?\(?\d{1,4}?\)?[\s\-]?\d{1,4}[\s\-]?\d{1,4}[\s\-]?\d{1,4}$",
            ErrorMessage = "Phone number must be in a valid international format. Example format: +44 20 7946 0958.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "WhatsApp number is required.")]
        [RegularExpression(@"^\+?(\d{1,4})?[\s\-]?\(?\d{1,4}?\)?[\s\-]?\d{1,4}[\s\-]?\d{1,4}[\s\-]?\d{1,4}$",
            ErrorMessage = "WhatsApp number must be in a valid international format. Example format: +44 20 7946 0958.")]
        public string WhatsappNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
        public string Password { get; set; }

        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public AddressRequestDto Address { get; set; }
    }

    public class AddressRequestDto
    {
        [Required(ErrorMessage = "Address Line 1 is required.")]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip code is required.")]
        [RegularExpression(@"^\d{5}(-\d{4})?$",
            ErrorMessage = "Invalid zip code format. Example: 12345 or 12345-6789.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }
    }


}
