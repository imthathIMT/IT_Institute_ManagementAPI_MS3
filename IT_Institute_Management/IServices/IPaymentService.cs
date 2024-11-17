using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync();
        Task<PaymentResponseDto> GetPaymentByIdAsync(Guid id);
        Task<IEnumerable<PaymentResponseDto>> GetPaymentsByStudentNICAsync(string nic);
        Task CreatePaymentAsync(PaymentRequestDto paymentRequestDto);
        Task UpdatePaymentAsync(Guid id, PaymentRequestDto paymentRequestDto);
        Task DeletePaymentAsync(Guid id);
    }
}
