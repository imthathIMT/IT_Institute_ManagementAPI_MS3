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

        // Inject services through constructor
        public SummeryController(IStudentService studentService, ICourseService courseService, IEnrollmentService enrollmentService, IPaymentService paymentService)
        {
            _studentService = studentService;
            _courseService = courseService;
            _enrollmentService = enrollmentService;
            _paymentService = paymentService;
        }

        // Get the summary (Total Students and Total Courses)
        [HttpGet("summary")]
        public async Task<ActionResult<SummaryResponseDto>> GetSummary()
        {
            // Fetch students and courses
            var students = await _studentService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();

            // Create summary response DTO
            var summary = new SummaryResponseDto
            {
                TotalStudents = students.Count,
                TotalCourses = courses.Count()
            };

            return Ok(summary);
        }

        // Get Enrollment Summary (Total, Complete, and Reading Enrollments)
        [HttpGet("enrollment-summary")]
        public async Task<ActionResult<EnrollmentSummaryResponseDto>> GetEnrollmentSummary()
        {
            // Fetch all enrollments
            var allEnrollments = await _enrollmentService.GetAllEnrollmentsAsync();

            // Fetch complete enrollments
            var completeEnrollments = await _enrollmentService.GetEnrollmentsByCompletionStatusAsync(true);

            // Fetch reading enrollments
            var readingEnrollments = await _enrollmentService.GetEnrollmentsByCompletionStatusAsync(false);

            // Create the enrollment summary response DTO
            var summary = new EnrollmentSummaryResponseDto
            {
                TotalEnrollments = allEnrollments.Count(),
                CompleteEnrollments = completeEnrollments.Count(),
                ReadingEnrollments = readingEnrollments.Count()
            };

            return Ok(summary);
        }

        // Get Revenue Summary (Total Revenue, Current Year Revenue, Current Month Revenue)
        [HttpGet("revenue-summary")]
        //[Authorize(Roles = "MasterAdmin, Admin")]
        public async Task<ActionResult<RevenueSummaryResponseDto>> GetRevenueSummary()
        {
            // Fetch all payments
            var allPayments = await _paymentService.GetAllPaymentsAsync();

            // Calculate total revenue
            var totalRevenue = allPayments.Sum(p => p.Amount);

            // Get current year (we can use DateTime.Now.Year)
            var currentYear = DateTime.Now.Year;
            var currentYearPayments = allPayments.Where(p => p.PaymentDate.Year == currentYear);
            var currentYearRevenue = currentYearPayments.Sum(p => p.Amount);

            // Get current month (we can use DateTime.Now.Month)
            var currentMonth = DateTime.Now.Month;
            var currentMonthPayments = currentYearPayments.Where(p => p.PaymentDate.Month == currentMonth);
            var currentMonthRevenue = currentMonthPayments.Sum(p => p.Amount);

            // Create the revenue summary response DTO
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
