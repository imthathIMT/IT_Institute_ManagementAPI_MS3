using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync();
        Task<PaymentResponseDto> GetPaymentByIdAsync(Guid id);
    }
}
