using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(Guid id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                return Ok(payment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Payment not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("student/{nic}")]
        public async Task<IActionResult> GetPaymentsByStudentNIC(string nic)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByStudentNICAsync(nic);
                return Ok(payments);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestDto paymentRequestDto)
        {
            try
            {
                await _paymentService.CreatePaymentAsync(paymentRequestDto);
                return CreatedAtAction(nameof(GetPayment), new { id = paymentRequestDto.EnrollmentId }, paymentRequestDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(Guid id, [FromBody] PaymentRequestDto paymentRequestDto)
        {
            try
            {
                await _paymentService.UpdatePaymentAsync(id, paymentRequestDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Payment not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(Guid id)
        {
            try
            {
                await _paymentService.DeletePaymentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Payment not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Payment/total-income
        [HttpGet("total-income")]
        public async Task<IActionResult> GetTotalIncome()
        {
            try
            {
                var totalIncome = await _paymentService.GetTotalIncomeAsync();
                return Ok(new { TotalIncome = totalIncome });
            }
            catch (Exception ex)
            {
                // Log the error if needed
                return BadRequest(new { Message = "An error occurred while calculating total income.", Details = ex.Message });
            }
        }

    }
}
