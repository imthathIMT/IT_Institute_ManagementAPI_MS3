using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;

namespace IT_Institute_Management.Services
{
    public class SocialMediaLinksService : ISocialMediaLinksService
    {
        private readonly ISocialMediaLinksRepository _repository;

        public SocialMediaLinksService(ISocialMediaLinksRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SocialMediaLinksResponseDto>> GetAllAsync()
        {
            var socialMediaLinks = await _repository.GetAllAsync();
            return socialMediaLinks.Select(s => new SocialMediaLinksResponseDto
            {
                Id = s.Id,
                LinkedIn = s.LinkedIn,
                Instagram = s.Instagram,
                Facebook = s.Facebook,
                GitHub = s.GitHub,
                WhatsApp = s.WhatsApp,
                StudentNIC = s.StudentNIC
            });
        }

        public async Task<SocialMediaLinksResponseDto> GetByNICAsync(string nic)
        {
            var socialMediaLinks = await _repository.GetByNICAsync(nic);
            if (socialMediaLinks == null)
            {
                return null;
            }

            return new SocialMediaLinksResponseDto
            {
                Id = socialMediaLinks.Id,
                LinkedIn = socialMediaLinks.LinkedIn,
                Instagram = socialMediaLinks.Instagram,
                Facebook = socialMediaLinks.Facebook,
                GitHub = socialMediaLinks.GitHub,
                WhatsApp = socialMediaLinks.WhatsApp,
                StudentNIC = socialMediaLinks.StudentNIC
            };
        }

        public async Task<SocialMediaLinksResponseDto> CreateAsync(SocialMediaLinksRequestDto requestDto)
        {
            var entity = new SocialMediaLinks
            {
                LinkedIn = requestDto.LinkedIn,
                Instagram = requestDto.Instagram,
                Facebook = requestDto.Facebook,
                GitHub = requestDto.GitHub,
                WhatsApp = requestDto.WhatsApp,
                StudentNIC = requestDto.StudentNIC
            };

            var createdEntity = await _repository.CreateAsync(entity);
            return new SocialMediaLinksResponseDto
            {
                Id = createdEntity.Id,
                LinkedIn = createdEntity.LinkedIn,
                Instagram = createdEntity.Instagram,
                Facebook = createdEntity.Facebook,
                GitHub = createdEntity.GitHub,
                WhatsApp = createdEntity.WhatsApp,
                StudentNIC = createdEntity.StudentNIC
            };
        }

        public async Task<SocialMediaLinksResponseDto> UpdateAsync(Guid id, SocialMediaLinksRequestDto requestDto)
        {
            var entity = await _repository.GetByNICAsync(requestDto.StudentNIC);
            if (entity == null)
            {
                return null;
            }

            entity.LinkedIn = requestDto.LinkedIn;
            entity.Instagram = requestDto.Instagram;
            entity.Facebook = requestDto.Facebook;
            entity.GitHub = requestDto.GitHub;
            entity.WhatsApp = requestDto.WhatsApp;

            var updatedEntity = await _repository.UpdateAsync(entity);

            return new SocialMediaLinksResponseDto
            {
                Id = updatedEntity.Id,
                LinkedIn = updatedEntity.LinkedIn,
                Instagram = updatedEntity.Instagram,
                Facebook = updatedEntity.Facebook,
                GitHub = updatedEntity.GitHub,
                WhatsApp = updatedEntity.WhatsApp,
                StudentNIC = updatedEntity.StudentNIC
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}

