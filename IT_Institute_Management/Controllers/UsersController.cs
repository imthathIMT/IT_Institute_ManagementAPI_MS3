using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{nic}")]
        public async Task<IActionResult> GetUserById(string nic)
        {
            try
            {
                var user = await _userService.GetByIdAsync(nic);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRequestDto userDto, [FromQuery] Role role)
        {
            try
            {
                await _userService.AddAsync(userDto, role);
                return CreatedAtAction(nameof(GetUserById), new { nic = userDto.NIC }, userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{nic}")]
        public async Task<IActionResult> UpdateUser(string nic, [FromBody] UserRequestDto userDto)
        {
            try
            {
                await _userService.UpdateAsync(nic, userDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
