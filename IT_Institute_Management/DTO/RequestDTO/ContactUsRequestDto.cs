namespace IT_Institute_Management.DTO.RequestDTO
{
    public class ContactUsRequestDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

    }
}
