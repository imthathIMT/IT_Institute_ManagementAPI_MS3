using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IServices
{
    public interface IEnrollmentService
    {
        Task<Enrollment> CreateEnrollmentAsync(EnrollmentRequestDto enrollmentRequest);
        Task<Enrollment> DeleteEnrollmentByNICAsync(string nic, bool forceDelete = false);
        Task<Enrollment> UpdateEnrollmentCompletionStatus(Guid id);
    }
}
