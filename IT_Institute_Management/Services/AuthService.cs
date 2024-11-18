using Azure.Core;
using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using IT_Institute_Management.PasswordService;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IT_Institute_Management.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository,IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
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
                var token = CreateToken(user);
                return token;
            }
            else
            {
                throw new Exception("User not found");
            }           
        }



        private string CreateToken(User user)
        {
            var claimsList = new List<Claim>();
            claimsList.Add(new Claim("nic", user.NIC.ToString()));
            claimsList.Add(new Claim("Role", user.Role.ToString()));


            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var credintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
                claims: claimsList,
                expires: DateTime.Now.AddDays(10),
                signingCredentials: credintials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
