using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace IT_Institute_Management.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly InstituteDbContext _context;

        public PaymentRepository(InstituteDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments.Include(p => p.Enrollment).ToListAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            return await _context.Payments.Include(p => p.Enrollment).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreatePaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

    }
}
