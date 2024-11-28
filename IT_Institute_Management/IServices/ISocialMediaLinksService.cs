using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface ISocialMediaLinksService
    {
        Task<IEnumerable<SocialMediaLinksResponseDto>> GetAllAsync();
        Task<SocialMediaLinksResponseDto> GetByNICAsync(string nic);
        Task<SocialMediaLinksResponseDto> CreateAsync(SocialMediaLinksRequestDto requestDto);
        Task<SocialMediaLinksResponseDto> UpdateAsync(Guid id, SocialMediaLinksRequestDto requestDto);
        Task<bool> DeleteAsync(string nic);
    }
}
