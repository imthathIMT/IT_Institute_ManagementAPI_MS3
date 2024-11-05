using IT_Institute_Management.Database;
using IT_Institute_Management.IRepositories;

namespace IT_Institute_Management.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly InstituteDbContext _context;

        public PaymentRepository(InstituteDbContext context)
        {
            _context = context;
        }
    }
}
