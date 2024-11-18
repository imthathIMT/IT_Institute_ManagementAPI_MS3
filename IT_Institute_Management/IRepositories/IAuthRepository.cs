using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface IAuthRepository
    {
        Task<User> GetLoginUser(string nic);
    }
}
