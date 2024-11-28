namespace IT_Institute_Management.DTO.ResponseDTO
{
    public class StudentMessageResponseDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string StudentNIC { get; set; }
        public StudentResponseDto Student { get; set; }
    }
}
