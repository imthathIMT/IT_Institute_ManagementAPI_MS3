namespace IT_Institute_Management.DTO.ResponseDTO
{
    public class PaymentResponseDto
    {
        public Guid Id { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid EnrollmentId { get; set; }
        public EnrollmentResponseDto Enrollment { get; set; }
    }
}
