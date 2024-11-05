using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;

namespace IT_Institute_Management.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();

            return payments.Select(p => new PaymentResponseDto
            {
                Id = p.Id,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                FullAmount = p.FullAmount,
                DueAmount = p.DueAmount,
                EnrollmentId = (Guid)p.EnrollmentId
            });
        }
    }
}
