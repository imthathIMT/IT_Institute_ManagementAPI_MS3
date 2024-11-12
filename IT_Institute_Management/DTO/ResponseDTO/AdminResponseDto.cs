namespace IT_Institute_Management.DTO.ResponseDTO
{
    public class AdminResponseDto
    {
        public string NIC { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }

        // You can also include additional data that you want to return in the response, such as email, phone, etc.
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
