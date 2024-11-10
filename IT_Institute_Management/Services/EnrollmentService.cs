using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;

namespace IT_Institute_Management.Services
{
    public class EnrollmentService
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

        public async Task<Enrollment> DeleteEnrollmentByNICAsync(string nic, bool forceDelete = false)
        {
            var enrollment = await _repo.GetEnrollmentByNICAsync(nic);
            if (enrollment == null) throw new Exception("Enrollment not found.");

            if (!forceDelete)
            {
                // Check if it's more than a week old
                if (enrollment.EnrollmentDate.AddDays(7) > DateTime.Now)
                {
                    throw new Exception("Enrollment can only be deleted after a week from the enrollment date.");
                }
            }

            return await _repo.DeleteEnrollmentByNICAsync(nic);
        }

        public async Task<Enrollment> UpdateEnrollmentCompletionStatus(Guid id)
        {
            var enrollment = await _repo.GetEnrollmentByIdAsync(id);
            if (enrollment == null) throw new Exception("Enrollment not found.");

            // Check course duration
            var course = await _courseRepo.GetCourseByIdAsync(enrollment.CourseId);
            if (course == null) throw new Exception("Course not found.");

            var courseEndDate = enrollment.EnrollmentDate.AddDays(course.Duration);
            enrollment.IsComplete = DateTime.Now >= courseEndDate;

            await _repo.SaveChangesAsync();

            return enrollment;
        }
    }
}
