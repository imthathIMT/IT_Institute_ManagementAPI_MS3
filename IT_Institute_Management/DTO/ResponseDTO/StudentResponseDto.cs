namespace IT_Institute_Management.DTO.ResponseDTO
{
    public class StudentResponseDto
    {
        public string NIC { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsLocked { get; set; }
        public int FailedLoginAttempts { get; set; }
        public string ImagePath { get; set; }
        public AddressResponseDto Address { get; set; }
        public SocialMediaLinksResponseDto SocialMediaLinks { get; set; }
        public List<EnrollmentResponseDto> Enrollments { get; set; } // List to hold multiple enrollments

    }

    public class AddressResponseDto
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

}
