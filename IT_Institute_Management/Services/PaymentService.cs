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
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IStudentRepository _studentRepository;

        public PaymentService(IPaymentRepository paymentRepository, ICourseRepository courseRepository, IEnrollmentRepository enrollmentRepository, IStudentRepository studentRepository)
        {
            _paymentRepository = paymentRepository;
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();
            var paymentDtos = new List<PaymentResponseDto>();

            foreach (var p in payments)
            {
                var totalPaid = (await _paymentRepository.GetPaymentsByEnrollmentIdAsync(p.EnrollmentId.GetValueOrDefault())).Sum(payment => payment.Amount);
                var fullAmount = await CalculateFullAmountAsync(p.EnrollmentId.GetValueOrDefault());
                var dueAmount = fullAmount - totalPaid;

                
                dueAmount = dueAmount < 0 ? 0 : dueAmount;

                paymentDtos.Add(new PaymentResponseDto
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    EnrollmentId = p.EnrollmentId.GetValueOrDefault(),
                    TotalPaidAmount = totalPaid, 
                    DueAmount = dueAmount
                });
            }

            return paymentDtos;
        }

        public async Task<PaymentResponseDto> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);

            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            var totalPaid = (await _paymentRepository.GetPaymentsByEnrollmentIdAsync(payment.EnrollmentId.GetValueOrDefault())).Sum(p => p.Amount);
            var fullAmount = await CalculateFullAmountAsync(payment.EnrollmentId.GetValueOrDefault());
            var dueAmount = fullAmount - totalPaid;

           
            dueAmount = dueAmount < 0 ? 0 : dueAmount;

            return new PaymentResponseDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                EnrollmentId = payment.EnrollmentId.GetValueOrDefault(),
                TotalPaidAmount = totalPaid, 
                DueAmount = dueAmount
            };
        }



        public async Task CreatePaymentAsync(PaymentRequestDto paymentRequestDto)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(paymentRequestDto.EnrollmentId);
            if (enrollment == null)
                throw new KeyNotFoundException("Enrollment not found.");

            var course = await _courseRepository.GetCourseByIdAsync(enrollment.CourseId);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            var totalPaid = await GetTotalPaymentsAsync(paymentRequestDto.EnrollmentId);
            var fullAmount = course.Fees;
            var courseDurationMonths = course.Duration;
            var monthlyInstallment = fullAmount / courseDurationMonths;

           
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

               
                var lastPayment = (await _paymentRepository.GetPaymentsByEnrollmentIdAsync(paymentRequestDto.EnrollmentId))
                                    .OrderByDescending(p => p.PaymentDate)
                                    .FirstOrDefault();

                if (lastPayment != null)
                {
                    var nextPaymentDate = lastPayment.PaymentDate.AddMonths(1); 
                    if (paymentRequestDto.PaymentDate < nextPaymentDate)
                    {
                        
                        throw new InvalidOperationException($"Next installment can only be paid after 1 month from the previous payment. The next payment date is {nextPaymentDate:MMMM dd, yyyy}.");
                    }
                }

                var remainingMonths = courseDurationMonths - (totalPaid / monthlyInstallment);
                if (remainingMonths <= 0)
                {
                    throw new InvalidOperationException("All installments have been paid. No further payments are allowed.");
                }

                if (totalPaid + paymentRequestDto.Amount > fullAmount)
                {
                    throw new InvalidOperationException($"The total amount paid cannot exceed the course fee ({fullAmount:C}).");
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown payment plan type.");
            }

            
            var dueAmount = fullAmount - totalPaid - paymentRequestDto.Amount;

            
            if (dueAmount < 0)
            {
                throw new InvalidOperationException($"Amount paid exceeds the due amount for the course. Due amount: {fullAmount - totalPaid:C}");
            }

           
            var payment = new Payment
            {
                Amount = paymentRequestDto.Amount,
                TotalPaidAmount = totalPaid + paymentRequestDto.Amount, 
                DueAmount = dueAmount, 
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

            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(paymentRequestDto.EnrollmentId);
            if (enrollment == null)
                throw new KeyNotFoundException("Enrollment not found.");

            var course = await _courseRepository.GetCourseByIdAsync(enrollment.CourseId);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            var totalPaid = await GetTotalPaymentsAsync(paymentRequestDto.EnrollmentId);
            var fullAmount = course.Fees;
            var courseDurationMonths = course.Duration;
            var monthlyInstallment = fullAmount / courseDurationMonths;

          
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

               
                var lastPayment = (await _paymentRepository.GetPaymentsByEnrollmentIdAsync(paymentRequestDto.EnrollmentId))
                                    .OrderByDescending(p => p.PaymentDate)
                                    .FirstOrDefault();

                if (lastPayment != null)
                {
                    var nextPaymentDate = lastPayment.PaymentDate.AddMonths(1);
                    if (paymentRequestDto.PaymentDate < nextPaymentDate)
                    {
                        
                        throw new InvalidOperationException($"Next installment can only be paid after 1 month from the previous payment. The next payment date is {nextPaymentDate:MMMM dd, yyyy}.");
                    }
                }

                var remainingMonths = courseDurationMonths - (totalPaid / monthlyInstallment);
                if (remainingMonths <= 0)
                {
                    throw new InvalidOperationException("All installments have been paid. No further payments are allowed.");
                }

                if (totalPaid + paymentRequestDto.Amount > fullAmount)
                {
                    throw new InvalidOperationException($"The total amount paid cannot exceed the course fee ({fullAmount:C}).");
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown payment plan type.");
            }

           
            var dueAmount = fullAmount - totalPaid - paymentRequestDto.Amount;

            
            if (dueAmount < 0)
            {
                throw new InvalidOperationException($"Amount paid exceeds the due amount for the course. Due amount: {fullAmount - totalPaid:C}");
            }

           
            existingPayment.Amount = paymentRequestDto.Amount;
            existingPayment.TotalPaidAmount = totalPaid + paymentRequestDto.Amount;
            existingPayment.DueAmount = dueAmount;
            existingPayment.PaymentDate = paymentRequestDto.PaymentDate;

           
            await _paymentRepository.UpdatePaymentAsync(existingPayment);
        }



      
        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByStudentNICAsync(string nic)
        {
            
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
                throw new KeyNotFoundException("Student not found.");

            
            var enrollments = await _enrollmentRepository.GetEnrollmentByNICAsync(student.NIC);
            if (enrollments == null || !enrollments.Any())
                throw new KeyNotFoundException("No enrollments found for this student.");

           
            var paymentDtos = new List<PaymentResponseDto>();
            foreach (var enrollment in enrollments)
            {
               
                var payments = await _paymentRepository.GetPaymentsByEnrollmentIdAsync(enrollment.Id);
                foreach (var payment in payments)
                {
                   
                    var totalPaid = (await _paymentRepository.GetPaymentsByEnrollmentIdAsync(payment.EnrollmentId.GetValueOrDefault()))
                                    .Sum(p => p.Amount);

                   
                    var fullAmount = await CalculateFullAmountAsync(payment.EnrollmentId.GetValueOrDefault());

                   
                    var dueAmount = fullAmount - totalPaid;

                    
                    dueAmount = dueAmount < 0 ? 0 : dueAmount;

                   
                    paymentDtos.Add(new PaymentResponseDto
                    {
                        Id = payment.Id,
                        Amount = payment.Amount,
                        PaymentDate = payment.PaymentDate,
                        EnrollmentId = payment.EnrollmentId.GetValueOrDefault(),
                        TotalPaidAmount = totalPaid,
                        DueAmount = dueAmount
                    });
                }
            }

            return paymentDtos;
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
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(enrollmentId);
            var course = await _courseRepository.GetCourseByIdAsync(enrollment.CourseId);
            return course?.Fees ?? 0m;
        }

        private async Task<decimal> GetTotalPaymentsAsync(Guid enrollmentId)
        {
            var payments = await _paymentRepository.GetPaymentsByEnrollmentIdAsync(enrollmentId);
            return payments.Sum(p => p.Amount);
        }


    }
}
