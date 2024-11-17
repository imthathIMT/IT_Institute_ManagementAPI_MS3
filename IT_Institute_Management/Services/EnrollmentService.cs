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

        public EnrollmentService(IEnrollmentRepository repo, ICourseRepository courseRepo)
        {
            _repo = repo;
            _courseRepo = courseRepo;
        }

        public async Task<Enrollment> CreateEnrollmentAsync(EnrollmentRequestDto enrollmentRequest)
        {
            var course = await _courseRepo.GetCourseByIdAsync(enrollmentRequest.CourseId);
            if (course == null) throw new Exception("Course not found.");

            var enrollment = new Enrollment
            {
                Id = Guid.NewGuid(),
                EnrollmentDate = DateTime.Now,
                PaymentPlan = enrollmentRequest.PaymentPlan,
                StudentNIC = enrollmentRequest.StudentNIC,
                CourseId = enrollmentRequest.CourseId,
                IsComplete = false
            };

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
            if (enrollment == null) throw new Exception("Enrollment not found.");

            var course = await _courseRepo.GetCourseByIdAsync(enrollmentRequest.CourseId);
            if (course == null) throw new Exception("Course not found.");

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
                .Where(e => e.StudentNIC == nic && e.IsComplete && e.EnrollmentDate.AddDays(e.Course.Duration) <= DateTime.Now)
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

            return await _repo.DeleteEnrollmentAsync(id);
        }

    }
}
