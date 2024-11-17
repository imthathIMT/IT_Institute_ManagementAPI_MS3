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
        private readonly ICourseRepository _courseRepository;

        public PaymentService(IPaymentRepository paymentRepository, ICourseRepository courseRepository)
        {
            _paymentRepository = paymentRepository;
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();

            // Map to DTO and calculate FullAmount and DueAmount
            var paymentDtos = await Task.WhenAll(payments.Select(async p =>
            {
                var fullAmount = await CalculateFullAmountAsync(p.EnrollmentId.GetValueOrDefault());
                var dueAmount = fullAmount - payments.Where(payment => payment.EnrollmentId == p.EnrollmentId).Sum(payment => payment.Amount);
                return new PaymentResponseDto
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    EnrollmentId = p.EnrollmentId.GetValueOrDefault(),
                    FullAmount = fullAmount,
                    DueAmount = dueAmount
                };
            }));

            return paymentDtos;
        }

        public async Task<PaymentResponseDto> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);

            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            var fullAmount = await CalculateFullAmountAsync(payment.EnrollmentId.GetValueOrDefault());
            var dueAmount = fullAmount - (await _paymentRepository.GetPaymentsByStudentNICAsync(payment.Enrollment.StudentNIC)).Sum(p => p.Amount);

            return new PaymentResponseDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                EnrollmentId = payment.EnrollmentId.GetValueOrDefault(),
                FullAmount = fullAmount,
                DueAmount = dueAmount
            };
        }

        public async Task CreatePaymentAsync(PaymentRequestDto paymentRequestDto)
        {
            var payment = new Payment
            {
                Amount = paymentRequestDto.Amount,
                PaymentDate = paymentRequestDto.PaymentDate,
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

            await _paymentRepository.UpdatePaymentAsync(existingPayment);
        }

        public async Task DeletePaymentAsync(Guid id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);

            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            await _paymentRepository.DeletePaymentAsync(id);
        }

        private async Task<decimal> CalculateFullAmountAsync(Guid enrollmentId)
        {
            // Get course associated with the enrollmentId (you can fetch the course by EnrollmentId)
            var enrollment = await _paymentRepository.GetPaymentByIdAsync(enrollmentId);
            if (enrollment == null || enrollment.Enrollment == null)
                throw new KeyNotFoundException("Enrollment not found for this payment.");

            var course = await _courseRepository.GetCourseByIdAsync(enrollment.Enrollment.CourseId);

            return course?.Fees ?? 0m;
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByStudentNICAsync(string nic)
        {
            var payments = await _paymentRepository.GetPaymentsByStudentNICAsync(nic);

            var paymentDtos = await Task.WhenAll(payments.Select(async p =>
            {
                var fullAmount = await CalculateFullAmountAsync(p.EnrollmentId.GetValueOrDefault());
                var dueAmount = fullAmount - payments.Where(payment => payment.EnrollmentId == p.EnrollmentId).Sum(payment => payment.Amount);
                return new PaymentResponseDto
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    EnrollmentId = p.EnrollmentId.GetValueOrDefault(),
                    FullAmount = fullAmount,
                    DueAmount = dueAmount
                };
            }));

            return paymentDtos;
        }


    }
}
