using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentMessageController : ControllerBase
    {
        private readonly IStudentMessageService _service;

        public StudentMessageController(IStudentMessageService studentMessageService)
        {
            _service = studentMessageService;
        }
    }
}
