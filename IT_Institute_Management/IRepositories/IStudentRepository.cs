using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student> GetByNicAsync(string nic);  
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(string nic); 
    }
}
