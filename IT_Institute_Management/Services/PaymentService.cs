using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.Entity;
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


        public async Task<PaymentResponseDto> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);

            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            return new PaymentResponseDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                FullAmount = payment.FullAmount,
                DueAmount = payment.DueAmount,
                EnrollmentId = (Guid)payment.EnrollmentId
            };
        }


        public async Task CreatePaymentAsync(PaymentRequestDto paymentRequestDto)
        {
            var payment = new Payment
            {
                Amount = paymentRequestDto.Amount,
                PaymentDate = paymentRequestDto.PaymentDate,
                FullAmount = paymentRequestDto.FullAmount,
                DueAmount = paymentRequestDto.DueAmount,
                EnrollmentId = paymentRequestDto.EnrollmentId
            };

            await _paymentRepository.CreatePaymentAsync(payment);
        }

        public async Task UpdatePaymentAsync(Guid id, PaymentRequestDto paymentRequestDto)
        {
            var existingPayment = await _paymentRepository.GetPaymentByIdAsync(id);

            if (existingPayment == null)
                throw new KeyNotFoundException("Payment not found.");

            existingPayment.Amount = paymentRequestDto.Amount;
            existingPayment.PaymentDate = paymentRequestDto.PaymentDate;
            existingPayment.FullAmount = paymentRequestDto.FullAmount;
            existingPayment.DueAmount = paymentRequestDto.DueAmount;

            await _paymentRepository.UpdatePaymentAsync(existingPayment);
        }



        public async Task DeletePaymentAsync(Guid id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);

            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            await _paymentRepository.DeletePaymentAsync(id);
        }
    }
}
