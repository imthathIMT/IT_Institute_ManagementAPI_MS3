using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService) {
            _adminService = adminService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() { 
            var admins = await _adminService.GetAllAsync();
            return Ok(admins); 
        }
        [HttpGet("{nic}")]
        public async Task<IActionResult> GetById(string nic)
        {
            var admin = await _adminService.GetByIdAsync(nic);
            if (admin == null) { return NotFound(); }
            return Ok(admin);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AdminRequestDto adminDto)
        {
            await _adminService.AddAsync(adminDto);
            return CreatedAtAction(nameof(GetById), new { nic = adminDto.NIC }, adminDto);
        }


        .

    }
}
