namespace IT_Institute_Management.DTO.ResponseDTO
{
    public class NotificationResponseDTO
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string StudentNIC { get; set; }
    }
}
