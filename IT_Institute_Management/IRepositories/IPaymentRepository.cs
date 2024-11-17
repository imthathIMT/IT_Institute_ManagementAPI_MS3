using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(Guid id);
        Task<IEnumerable<Payment>> GetPaymentsByStudentNICAsync(string nic);
        Task CreatePaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(Guid id);
    }
}
