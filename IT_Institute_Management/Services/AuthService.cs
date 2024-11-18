using Azure.Core;
using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using IT_Institute_Management.PasswordService;

namespace IT_Institute_Management.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IAuthRepository authRepository,IPasswordHasher passwordHasher)
        {
            _authRepository = authRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> GetLoginUserToken(UserLoginModal request)
        {
            var user = await _authRepository.GetLoginUser(request.nic);

            if(user != null)
            {
                if (!_passwordHasher.VerifyHashedPassword(user.Password,request.Password))
                {
                    throw new Exception("Wrong password.");
                }
                var token = "Login successfull";
                return token;
            }
            else
            {
                throw new Exception("User not found");
            }           
        }

    }
}
