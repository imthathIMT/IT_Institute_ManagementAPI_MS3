using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface IAnnouncementRepository
    {
        Task<IEnumerable<Announcement>> GetAllAsync();
        Task<Announcement> GetByIdAsync(Guid id);


    }
}
