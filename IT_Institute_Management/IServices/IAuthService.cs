using IT_Institute_Management.DTO.RequestDTO;

namespace IT_Institute_Management.IServices
{
    public interface IAuthService
    {
        Task<string> GetLoginUserToken(UserLoginModal request);
    }
}
