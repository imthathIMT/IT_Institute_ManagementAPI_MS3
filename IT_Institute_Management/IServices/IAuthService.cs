using IT_Institute_Management.DTO.RequestDTO;

namespace IT_Institute_Management.IServices
{
    public interface IAuthService
    {
        Task<UserLoginModal> GetLoginUser(string nic);
    }
}
