using IT_Institute_Management.DTO.RequestDTO;
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

        public EnrollmentService(IEnrollmentRepository repo, ICourseRepository courseRepo, INotificationService notificationService)
        {
            _repo = repo;
            _courseRepo = courseRepo;
            _notificationService = notificationService;
        }

        public async Task<Enrollment> CreateEnrollmentAsync(EnrollmentRequestDto enrollmentRequest)
        {
            var course = await _courseRepo.GetCourseByIdAsync(enrollmentRequest.CourseId);
            if (course == null) throw new Exception("Course not found.");

            var existingEnrollment = await _repo.GetEnrollmentByNICAsync(enrollmentRequest.StudentNIC);
            if (existingEnrollment.Any(e => e.CourseId == enrollmentRequest.CourseId))
            {
                throw new Exception("Student is already enrolled in this course.");
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

           
            if (enrollmentRequest.PaymentPlan == "Full")
            {
                var paymentDueDate = enrollment.EnrollmentDate.AddDays(7);
                if (DateTime.Now > paymentDueDate)
                {
                    await _notificationService.CreateNotificationAsync(new NotificationRequestDTO
                    {
                        Message = "Full payment for the course is overdue.",
                        Date = DateTime.Now,
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
                        Date = DateTime.Now,
                        StudentNIC = enrollment.StudentNIC
                    });
                }
            }

            return await _repo.AddEnrollmentAsync(enrollment);
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
                            Date = DateTime.Now,
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
                            Date = DateTime.Now,
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
                .Where(e => e.StudentNIC == nic)
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
                Date = DateTime.Now,
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
