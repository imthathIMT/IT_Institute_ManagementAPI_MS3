namespace IT_Institute_Management.DTO.ResponseDTO
{
    public class EnrollmentResponseDto
    {
        public Guid Id { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        public string PaymentPlan { get; set; }
        public bool IsComplete { get; set; }
        public string StudentNIC { get; set; }
        public Guid CourseId { get; set; }

        public PaymentResponseDto? payments { get; set; }
    }
}
