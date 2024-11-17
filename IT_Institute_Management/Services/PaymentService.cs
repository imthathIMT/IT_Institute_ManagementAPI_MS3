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
        private readonly IEnrollmentRepository _enrollmentRepository; // Added to fetch enrollment details

        public PaymentService(IPaymentRepository paymentRepository, ICourseRepository courseRepository, IEnrollmentRepository enrollmentRepository)
        {
            _paymentRepository = paymentRepository;
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository; // Initialize the enrollment repository
        }

        // Get all payments
        public async Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();
            var paymentDtos = new List<PaymentResponseDto>();

            foreach (var p in payments)
            {
                var fullAmount = await CalculateFullAmountAsync(p.EnrollmentId.GetValueOrDefault());
                var totalPaid = (await _paymentRepository.GetPaymentsByEnrollmentIdAsync(p.EnrollmentId.GetValueOrDefault())).Sum(payment => payment.Amount);
                var dueAmount = fullAmount - totalPaid;

                // Ensure the due amount never goes negative
                dueAmount = dueAmount < 0 ? 0 : dueAmount;

                paymentDtos.Add(new PaymentResponseDto
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    EnrollmentId = p.EnrollmentId.GetValueOrDefault(),
                    FullAmount = fullAmount,
                    DueAmount = dueAmount
                });
            }

            return paymentDtos;
        }

        // Get a payment by ID
        public async Task<PaymentResponseDto> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);

            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            var fullAmount = await CalculateFullAmountAsync(payment.EnrollmentId.GetValueOrDefault());
            var totalPaid = (await _paymentRepository.GetPaymentsByEnrollmentIdAsync(payment.EnrollmentId.GetValueOrDefault())).Sum(p => p.Amount);
            var dueAmount = fullAmount - totalPaid;

            // Ensure the due amount never goes negative
            dueAmount = dueAmount < 0 ? 0 : dueAmount;

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

        // Create a new payment
        public async Task CreatePaymentAsync(PaymentRequestDto paymentRequestDto)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(paymentRequestDto.EnrollmentId);
            if (enrollment == null)
                throw new KeyNotFoundException("Enrollment not found.");

            var course = await _courseRepository.GetCourseByIdAsync(enrollment.CourseId);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            var fullAmount = course.Fees;
            var courseDurationMonths = course.Duration;
            var monthlyInstallment = fullAmount / courseDurationMonths;

            var totalPaid = await GetTotalPaymentsAsync(paymentRequestDto.EnrollmentId);

            // Full Payment Validation
            if (enrollment.PaymentPlan == "Full")
            {
                if (totalPaid > 0)
                {
                    throw new InvalidOperationException("Full payment has already been made. No further payments are allowed.");
                }

                if (paymentRequestDto.Amount != fullAmount)
                {
                    throw new InvalidOperationException($"For full payment, the amount must be equal to the full course fee ({fullAmount:C}).");
                }
            }
            // Installment Payment Validation
            else if (enrollment.PaymentPlan == "Installment")
            {
                if (paymentRequestDto.Amount < monthlyInstallment)
                {
                    throw new InvalidOperationException($"Installment amount must be at least {monthlyInstallment:C}.");
                }

                if (paymentRequestDto.Amount > monthlyInstallment)
                {
                    throw new InvalidOperationException($"Installment amount cannot exceed {monthlyInstallment:C}.");
                }

                if (totalPaid + paymentRequestDto.Amount > fullAmount)
                {
                    throw new InvalidOperationException($"The total amount paid cannot exceed the course fee ({fullAmount:C}).");
                }

                var remainingMonths = courseDurationMonths - (totalPaid / monthlyInstallment);
                if (remainingMonths <= 0)
                {
                    throw new InvalidOperationException("All installments have been paid. No further payments are allowed.");
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown payment plan type.");
            }

            var payment = new Payment
            {
                Amount = paymentRequestDto.Amount,
                FullAmount = fullAmount,  
                DueAmount = fullAmount - totalPaid - paymentRequestDto.Amount,
                PaymentDate = paymentRequestDto.PaymentDate,
                EnrollmentId = paymentRequestDto.EnrollmentId
            };

            // After the payment is validated, we now reduce the due amount
            var dueAmount = fullAmount - totalPaid - paymentRequestDto.Amount;
            if (dueAmount < 0)
            {
                throw new InvalidOperationException("Amount paid exceeds the course fee.");
            }

            await _paymentRepository.CreatePaymentAsync(payment);
        }

        // Update an existing payment
        public async Task UpdatePaymentAsync(Guid id, PaymentRequestDto paymentRequestDto)
        {
            var existingPayment = await _paymentRepository.GetPaymentByIdAsync(id);

            if (existingPayment == null)
                throw new KeyNotFoundException("Payment not found.");

            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(paymentRequestDto.EnrollmentId);
            if (enrollment == null)
                throw new KeyNotFoundException("Enrollment not found.");

            var course = await _courseRepository.GetCourseByIdAsync(enrollment.CourseId);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            var fullAmount = course.Fees;
            var courseDurationMonths = course.Duration;
            var monthlyInstallment = fullAmount / courseDurationMonths;

            var totalPaid = await GetTotalPaymentsAsync(paymentRequestDto.EnrollmentId);

            // Full Payment Validation
            if (enrollment.PaymentPlan == "Full")
            {
                if (totalPaid > 0)
                {
                    throw new InvalidOperationException("Full payment has already been made. No further payments are allowed.");
                }

                if (paymentRequestDto.Amount != fullAmount)
                {
                    throw new InvalidOperationException($"For full payment, the amount must be equal to the full course fee ({fullAmount:C}).");
                }
            }
            // Installment Payment Validation
            else if (enrollment.PaymentPlan == "Installment")
            {
                if (paymentRequestDto.Amount < monthlyInstallment)
                {
                    throw new InvalidOperationException($"Installment amount must be at least {monthlyInstallment:C}.");
                }

                if (paymentRequestDto.Amount > monthlyInstallment)
                {
                    throw new InvalidOperationException($"Installment amount cannot exceed {monthlyInstallment:C}.");
                }

                if (totalPaid + paymentRequestDto.Amount > fullAmount)
                {
                    throw new InvalidOperationException($"The total amount paid cannot exceed the course fee ({fullAmount:C}).");
                }

                var remainingMonths = courseDurationMonths - (totalPaid / monthlyInstallment);
                if (remainingMonths <= 0)
                {
                    throw new InvalidOperationException("All installments have been paid. No further payments are allowed.");
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown payment plan type.");
            }

            existingPayment.Amount = paymentRequestDto.Amount;
            existingPayment.PaymentDate = paymentRequestDto.PaymentDate;

            await _paymentRepository.UpdatePaymentAsync(existingPayment);
        }

        // Delete a payment by ID
        public async Task DeletePaymentAsync(Guid id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);

            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            await _paymentRepository.DeletePaymentAsync(id);
        }

        // Calculate full amount based on course fees
        private async Task<decimal> CalculateFullAmountAsync(Guid enrollmentId)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(enrollmentId);

            if (enrollment == null)
            {
                throw new KeyNotFoundException("Enrollment not found for this payment.");
            }

            var course = await _courseRepository.GetCourseByIdAsync(enrollment.CourseId);
            return course?.Fees ?? 0m;
        }

        // Get payments by Student NIC
        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByStudentNICAsync(string nic)
        {
            var payments = await _paymentRepository.GetPaymentsByStudentNICAsync(nic);

            if (payments == null || !payments.Any())
            {
                throw new KeyNotFoundException("Payments not found for this student.");
            }

            var paymentDtos = new List<PaymentResponseDto>();

            foreach (var p in payments)
            {
                var fullAmount = await CalculateFullAmountAsync(p.EnrollmentId.GetValueOrDefault());
                var totalPaid = (await _paymentRepository.GetPaymentsByEnrollmentIdAsync(p.EnrollmentId.GetValueOrDefault())).Sum(payment => payment.Amount);
                var dueAmount = fullAmount - totalPaid;

                // Ensure the due amount never goes negative
                dueAmount = dueAmount < 0 ? 0 : dueAmount;

                paymentDtos.Add(new PaymentResponseDto
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    EnrollmentId = p.EnrollmentId.GetValueOrDefault(),
                    FullAmount = fullAmount,
                    DueAmount = dueAmount
                });
            }

            return paymentDtos;
        }

        // Calculate the total payments made for a specific enrollment
        private async Task<decimal> GetTotalPaymentsAsync(Guid enrollmentId)
        {
            var payments = await _paymentRepository.GetPaymentsByEnrollmentIdAsync(enrollmentId);
            return payments.Sum(p => p.Amount);
        }


    }
}
