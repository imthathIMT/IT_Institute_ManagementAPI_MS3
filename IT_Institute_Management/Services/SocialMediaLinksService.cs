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
            // Fetch all social media links
            var socialMediaLinks = await _repository.GetAllAsync();

            // Check if the result is null or empty
            if (socialMediaLinks == null || !socialMediaLinks.Any())
            {
                throw new Exception("Data not found");
            }

            // Transform and return the data
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
            // Check if NIC is null or empty
            if (string.IsNullOrWhiteSpace(nic))
            {
                throw new ArgumentException("NIC is required", nameof(nic));
            }

            // Fetch social media links by NIC
            var socialMediaLinks = await _repository.GetByNICAsync(nic);

            // Check if data is null or empty
            if (socialMediaLinks == null)
            {
                throw new InvalidOperationException("Data not found");
            }

            // Return the transformed DTO
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
            // Validate the StudentNIC in the request
            if (string.IsNullOrWhiteSpace(requestDto.StudentNIC))
            {
                throw new Exception("NIC is required");
            }

            // Map the request DTO to the entity
            var entity = new SocialMediaLinks
            {
                LinkedIn = requestDto.LinkedIn,
                Instagram = requestDto.Instagram,
                Facebook = requestDto.Facebook,
                GitHub = requestDto.GitHub,
                WhatsApp = requestDto.WhatsApp,
                StudentNIC = requestDto.StudentNIC
            };

            // Create the entity in the repository
            var createdEntity = await _repository.CreateAsync(entity);

            // Map the created entity to the response DTO
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
            // Validate if the NIC is null or empty
            if (string.IsNullOrWhiteSpace(requestDto.StudentNIC))
            {
                throw new ArgumentException("NIC is required.", nameof(requestDto.StudentNIC));
            }

            // Fetch the entity by NIC
            var entity = await _repository.GetByNICAsync(requestDto.StudentNIC);

            // Check if the entity is null and throw an exception
            if (entity == null)
            {
                throw new KeyNotFoundException($"No data found with NIC: {requestDto.StudentNIC}");
            }

            // Update social media links
            entity.LinkedIn = requestDto.LinkedIn;
            entity.Instagram = requestDto.Instagram;
            entity.Facebook = requestDto.Facebook;
            entity.GitHub = requestDto.GitHub;
            entity.WhatsApp = requestDto.WhatsApp;

            // Update the entity in the repository
            var updatedEntity = await _repository.UpdateAsync(entity);

            // Return the updated response DTO
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

