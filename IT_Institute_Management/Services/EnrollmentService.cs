using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.EmailSection.Models;
using IT_Institute_Management.EmailSection.Service;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;

namespace IT_Institute_Management.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repo;
        private readonly ICourseRepository _courseRepo;
        private readonly INotificationService _notificationService;
        private readonly IStudentRepository _studentRepository;
        private readonly sendmailService _sendmailService;

        public EnrollmentService(IEnrollmentRepository repo, ICourseRepository courseRepo, INotificationService notificationService,IStudentRepository studentRepository, sendmailService sendmailService)
        {
            _repo = repo;
            _courseRepo = courseRepo;
            _notificationService = notificationService;
            _studentRepository = studentRepository;
            _sendmailService = sendmailService;
        }


        public async Task<Enrollment> CreateEnrollmentAsync(EnrollmentRequestDto enrollmentRequest)
        {
            var course = await _courseRepo.GetCourseByIdAsync(enrollmentRequest.CourseId);
            if (course == null) throw new Exception("Course not found.");

            var existingEnrollment = await _repo.GetEnrollmentByNICAsync(enrollmentRequest.StudentNIC);
            if (existingEnrollment.Any(e => e.CourseId == enrollmentRequest.CourseId))
            {
                throw new Exception("Student  is already enrolled in this course.");
            }

            if (enrollmentRequest.PaymentPlan != "Full" && enrollmentRequest.PaymentPlan != "Installment")
            {
                throw new Exception("Payment plan must be either 'Full' or 'Installment'.");
            }

            var enrollment = new Enrollment
            {
                Id = Guid.NewGuid(),
                EnrollmentDate = DateTime.Now,
                PaymentPlan = enrollmentRequest.PaymentPlan,
                StudentNIC = enrollmentRequest.StudentNIC,
                CourseId = enrollmentRequest.CourseId,
                IsComplete = false
            };


            
            var paymentDueDate = enrollment.EnrollmentDate.AddDays(8);
            if (DateTime.Now > paymentDueDate)
            {
               
                await _repo.DeleteEnrollmentAsync(enrollment.Id);

                
                await _notificationService.CreateNotificationAsync(new NotificationRequestDTO
                {
                    Message = "Your enrollment has been deleted due to missed payment deadlines.",
                    StudentNIC = enrollment.StudentNIC
                });
            }
            else
            {
                var student = await _studentRepository.GetByNicAsync(enrollment.StudentNIC);
                if (student == null)
                {
                    throw new Exception($"Student with NIC {enrollment.StudentNIC} not found.");
                }
                var sendMailRequest = new SendMailRequest
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    CourseName = course.CourseName,
                    Duration = course.Duration,
                    Level = course.Level,
                    Fees = course.Fees,
                    StartDate = enrollment.EnrollmentDate,
                    PaymentPlan = enrollment.PaymentPlan,

                    TemplateName = "EnrollmentConfirmation"

                };

                if (_sendmailService == null)
                {
                    throw new InvalidOperationException("_sendmailService is not initialized.");
                }

                // Uncomment the email service once setup is correct
                // _emailService.SendRegistraionMail(studentDto.Email, studentDto);
                await _sendmailService.Sendmail(sendMailRequest);

                return await _repo.AddEnrollmentAsync(enrollment);
                
            }

            return null!;
        }

        public async Task<Enrollment> UpdateEnrollmentCompletionStatus(Guid id)
        {
            var enrollment = await _repo.GetEnrollmentByIdAsync(id);
            if (enrollment == null) throw new Exception("Enrollment not found.");

            var course = await _courseRepo.GetCourseByIdAsync(enrollment.CourseId);
            if (course == null) throw new Exception("Course not found.");

            var courseEndDate = enrollment.EnrollmentDate.AddDays(course.Duration);
            enrollment.IsComplete = DateTime.Now >= courseEndDate;

            await _repo.SaveChangesAsync();

            return enrollment;
        }



        public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _repo.GetAllEnrollmentsAsync();
        }




        public async Task<Enrollment> UpdateEnrollmentDataAsync(Guid id, EnrollmentRequestDto enrollmentRequest)
        {
            
            var enrollment = await _repo.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
            {
                throw new Exception("Enrollment not found.");
            }

            
            var course = await _courseRepo.GetCourseByIdAsync(enrollmentRequest.CourseId);
            if (course == null)
            {
                throw new Exception("Course not found.");
            }

           
            if (enrollmentRequest.PaymentPlan != "Full" && enrollmentRequest.PaymentPlan != "Installment")
            {
                throw new Exception("Payment plan must be either 'Full' or 'Installment'.");
            }

           
            if (enrollment.PaymentPlan != enrollmentRequest.PaymentPlan)
            {
               
                if (enrollmentRequest.PaymentPlan == "Full")
                {
                    var paymentDueDate = enrollment.EnrollmentDate.AddDays(7);
                    if (DateTime.Now > paymentDueDate)
                    {
                        await _notificationService.CreateNotificationAsync(new NotificationRequestDTO
                        {
                            Message = "Full payment for the course is overdue.",
                            StudentNIC = enrollment.StudentNIC
                        });
                    }
                }
                else if (enrollmentRequest.PaymentPlan == "Installment")
                {
                    var firstInstallmentDueDate = enrollment.EnrollmentDate.AddDays(7);
                    if (DateTime.Now > firstInstallmentDueDate)
                    {
                        await _notificationService.CreateNotificationAsync(new NotificationRequestDTO
                        {
                            Message = "First installment payment is overdue.",
                            StudentNIC = enrollment.StudentNIC
                        });
                    }
                }
            }

           
            enrollment.PaymentPlan = enrollmentRequest.PaymentPlan;
            enrollment.StudentNIC = enrollmentRequest.StudentNIC;
            enrollment.CourseId = enrollmentRequest.CourseId;

           
            await _repo.SaveChangesAsync();

            return enrollment;
        }


        public async Task<Enrollment> GetEnrollmentByIdAsync(Guid id)
        {
            var enrollment = await _repo.GetEnrollmentByIdAsync(id);
            if (enrollment == null) throw new Exception("Enrollment not found.");
            return enrollment;
        }


        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByNICAsync(string nic)
        {
            var enrollments = await _repo.GetAllEnrollmentsAsync();
            var filteredEnrollments = enrollments
                .Where(e => e.Student.NIC == nic)
                .ToList();

            if (!filteredEnrollments.Any())
            {
                throw new Exception("No completed enrollments found for the given NIC.");
            }

            return filteredEnrollments;
        }



        public async Task<Enrollment> DeleteEnrollmentByIdAsync(Guid id, bool forceDelete = false)
        {
            var enrollment = await _repo.GetEnrollmentByIdAsync(id);
            if (enrollment == null) throw new Exception("Enrollment not found.");

            if (!forceDelete && enrollment.EnrollmentDate.AddDays(7) > DateTime.Now)
            {
                throw new Exception("Enrollment can only be deleted after a week from the enrollment date.");
            }

            await _notificationService.CreateNotificationAsync(new NotificationRequestDTO
            {
                Message = "Your enrollment has been deleted.",
                StudentNIC = enrollment.StudentNIC
            });

            return await _repo.DeleteEnrollmentAsync(id);
        }



        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCompletionStatusAsync(bool isComplete)
        {
            return await _repo.GetEnrollmentsByCompletionStatusAsync(isComplete);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByNICAndCompletionStatusAsync(string nic, bool isComplete)
        {
            var enrollments = await _repo.GetEnrollmentsByNICAndCompletionStatusAsync(nic, isComplete);
            if (!enrollments.Any())
            {
                throw new Exception("No enrollments found for the given NIC and completion status.");
            }
            return enrollments;
        }

    }
}
