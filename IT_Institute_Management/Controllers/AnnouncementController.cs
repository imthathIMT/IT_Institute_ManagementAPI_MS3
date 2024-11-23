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
            try
            {
                var announcements = await _announcementService.GetAllAsync();
                if (announcements == null)
                {
                    return NotFound();
                }
                return Ok(announcements);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var announcement = await _announcementService.GetByIdAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            return Ok(announcement);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AnnouncementRequestDto announcementDto)
        {
            try
            {
                await _announcementService.AddAsync(announcementDto);
                return Ok("Announcement is posted successfull");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AnnouncementRequestDto announcementDto)
        {
            try
            {
                await _announcementService.UpdateAsync(id, announcementDto);
                return Ok("Announcement successfully updated");

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _announcementService.DeleteAsync(id);
                return Ok("Successfuly deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }


    }
}
