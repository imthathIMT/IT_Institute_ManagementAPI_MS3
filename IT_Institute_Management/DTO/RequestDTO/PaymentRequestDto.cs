namespace IT_Institute_Management.DTO.RequestDTO
{
    public class PaymentRequestDto
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal FullAmount { get; set; }
        public decimal DueAmount { get; set; }
        public Guid EnrollmentId { get; set; }
    }
}
