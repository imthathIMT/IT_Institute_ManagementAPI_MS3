using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface ISocialMediaLinksRepository
    {
        Task<IEnumerable<SocialMediaLinks>> GetAllAsync();
        Task<SocialMediaLinks> GetByNICAsync(string nic);
        Task<SocialMediaLinks> CreateAsync(SocialMediaLinks socialMediaLinks);
        Task<SocialMediaLinks> UpdateAsync(SocialMediaLinks socialMediaLinks);
        Task<bool> DeleteAsync(Guid id);
    }
}
