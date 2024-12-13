using IT_Institute_Management.DTO.ResponseDTO.SummeryDTO;
using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummeryController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IPaymentService _paymentService;

      
        public SummeryController(IStudentService studentService, ICourseService courseService, IEnrollmentService enrollmentService, IPaymentService paymentService)
        {
            _studentService = studentService;
            _courseService = courseService;
            _enrollmentService = enrollmentService;
            _paymentService = paymentService;
        }

        
        [HttpGet("summary")]
        public async Task<ActionResult<SummaryResponseDto>> GetSummary()
        {
          
            var students = await _studentService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();

           
            var summary = new SummaryResponseDto
            {
                TotalStudents = students.Count,
                TotalCourses = courses.Count()
            };

            return Ok(summary);
        }

        
        [HttpGet("enrollment-summary")]
        public async Task<ActionResult<EnrollmentSummaryResponseDto>> GetEnrollmentSummary()
        {
           
            var allEnrollments = await _enrollmentService.GetAllEnrollmentsAsync();

          
            var completeEnrollments = await _enrollmentService.GetEnrollmentsByCompletionStatusAsync(true);

        
            var readingEnrollments = await _enrollmentService.GetEnrollmentsByCompletionStatusAsync(false);

       
            var summary = new EnrollmentSummaryResponseDto
            {
                TotalEnrollments = allEnrollments.Count(),
                CompleteEnrollments = completeEnrollments.Count(),
                ReadingEnrollments = readingEnrollments.Count()
            };

            return Ok(summary);
        }

       
        [HttpGet("revenue-summary")]
      
        public async Task<ActionResult<RevenueSummaryResponseDto>> GetRevenueSummary()
        {
         
            var allPayments = await _paymentService.GetAllPaymentsAsync();

         
            var totalRevenue = allPayments.Sum(p => p.Amount);

        
            var currentYear = DateTime.Now.Year;
            var currentYearPayments = allPayments.Where(p => p.PaymentDate.Year == currentYear);
            var currentYearRevenue = currentYearPayments.Sum(p => p.Amount);

          
            var currentMonth = DateTime.Now.Month;
            var currentMonthPayments = currentYearPayments.Where(p => p.PaymentDate.Month == currentMonth);
            var currentMonthRevenue = currentMonthPayments.Sum(p => p.Amount);

         
            var revenueSummary = new RevenueSummaryResponseDto
            {
                TotalRevenue = totalRevenue,
                CurrentYearRevenue = currentYearRevenue,
                CurrentMonthRevenue = currentMonthRevenue
            };

            return Ok(revenueSummary);
        }
    }
}
