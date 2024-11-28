using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface IStudentMessageRepository
    {
        Task<IEnumerable<StudentMessage>> GetAllAsync();
        Task<IEnumerable<StudentMessage>> GetByStudentNICAsync(string studentNIC);
        Task<StudentMessage> AddAsync(StudentMessage studentMessage);
        Task DeleteAsync(Guid id);
        Task<bool> SaveAsync();
    }
}
