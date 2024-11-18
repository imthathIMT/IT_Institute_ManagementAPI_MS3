using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.IServices;
using IT_Institute_Management.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(UserLoginModal request)
        {
            var user = await _authService.GetLoginUser(request.nic);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                throw new Exception("Wrong password.");
            }
            var token = "Login successfull";
            return Ok(token);
        }
    }
}
