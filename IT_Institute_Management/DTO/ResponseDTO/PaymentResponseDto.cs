namespace IT_Institute_Management.DTO.ResponseDTO
{
    public class PaymentResponseDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal FullAmount { get; set; }
        public decimal DueAmount { get; set; }
        public Guid EnrollmentId { get; set; }
    }
}
