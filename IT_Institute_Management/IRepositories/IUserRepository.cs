using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string nic);
    }
}
