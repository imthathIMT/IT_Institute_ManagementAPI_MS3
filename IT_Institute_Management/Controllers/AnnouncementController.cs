using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;
        public AnnouncementController(IAnnouncementService announcementService) 
        {
            _announcementService = announcementService;
        }
        [HttpGet] 
        public async Task<IActionResult> GetAll() 
        {
            var announcements = await _announcementService.GetAllAsync();
            return Ok(announcements);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var announcement = await _announcementService.GetByIdAsync(id);
            if (announcement == null) {
                return NotFound(); 
            } 
            return Ok(announcement); 
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AnnouncementRequestDto announcementDto)
        {
            await _announcementService.AddAsync(announcementDto);
            return CreatedAtAction(nameof(GetById),new { id = Guid.NewGuid() }, announcementDto); }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AnnouncementRequestDto announcementDto) 
        { 
            await _announcementService.UpdateAsync(id, announcementDto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) 
        {
            await _announcementService.DeleteAsync(id); 
            return NoContent();
        }
    }
}
