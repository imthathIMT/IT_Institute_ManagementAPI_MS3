using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;

namespace IT_Institute_Management.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IContactUsRepository _contactUsRepository;

        public ContactUsService(IContactUsRepository contactUsRepository)
        {
            _contactUsRepository = contactUsRepository;
        }

        public async Task<IEnumerable<ContactUsResponseDto>> GetAllAsync()
        {
            var contacts = await _contactUsRepository.GetAllAsync();
            return contacts.Select(c => new ContactUsResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Message = c.Message,
                Date = c.Date
            });
        }
        public async Task<ContactUsResponseDto> GetByIdAsync(Guid id)
        {
            var contact = await _contactUsRepository.GetByIdAsync(id);
            return new ContactUsResponseDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Message = contact.Message,
                Date = contact.Date
            };
        }
        public async Task AddAsync(ContactUsRequestDto contactUsDto)
        {
            var contactUs = new ContactUs
            {
                Id = Guid.NewGuid(),
                Name = contactUsDto.Name,
                Email = contactUsDto.Email,
                Message = contactUsDto.Message,
                Date = contactUsDto.Date
            };
            await _contactUsRepository.AddAsync(contactUs);
        }


    }
}
