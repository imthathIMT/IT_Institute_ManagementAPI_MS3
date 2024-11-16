using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment);
        Task<Enrollment> GetEnrollmentByIdAsync(Guid id);
        Task<Enrollment> GetEnrollmentByNICAsync(string nic);
        Task<Enrollment> DeleteEnrollmentAsync(Guid id);
        Task<Enrollment> DeleteEnrollmentByNICAsync(string nic);
        Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync();
        Task SaveChangesAsync();
    }
}
