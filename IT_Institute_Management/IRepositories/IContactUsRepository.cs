using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface IContactUsRepository
    {
        Task<IEnumerable<ContactUs>> GetAllAsync();
        Task<ContactUs> GetByIdAsync(Guid id);
    }
}
