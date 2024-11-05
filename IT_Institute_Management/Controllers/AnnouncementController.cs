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
       
    }
}
