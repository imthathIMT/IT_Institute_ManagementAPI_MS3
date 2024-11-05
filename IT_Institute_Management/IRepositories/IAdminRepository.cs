using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Admin>> GetAllAsync();
        Task<Admin> GetByIdAsync(string nic);
        Task AddAsync(Admin admin);
    }
}
