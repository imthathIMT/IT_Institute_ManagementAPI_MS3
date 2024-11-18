using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;

namespace IT_Institute_Management.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<UserLoginModal> GetLoginUser(string nic)
        {
            var user = await _authRepository.GetLoginUser(nic);

            if(user != null)
            {
                var response = new UserLoginModal()
                {
                    nic = user.NIC,
                    Password = user.Password,
                };

                return response;
            }
            else
            {
                throw new Exception("User not found");
            }


            

        }

    }
}
