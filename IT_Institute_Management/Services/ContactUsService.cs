using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.EmailSerivice;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IContactUsRepository _contactUsRepository;
        private readonly IEmailService _emailService;

        public ContactUsService(IContactUsRepository contactUsRepository,IEmailService emailService)
        {
            _contactUsRepository = contactUsRepository;
            _emailService = emailService;
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
        public async Task UpdateAsync(Guid id, ContactUsRequestDto contactUsDto)
        {
            var contactUs = new ContactUs
            {
                Id = id,
                Name = contactUsDto.Name,
                Email = contactUsDto.Email,
                Message = contactUsDto.Message,
                Date = contactUsDto.Date
            };
            await _contactUsRepository.UpdateAsync(contactUs);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _contactUsRepository.DeleteAsync(id);
        }

        public async Task<string> ReplyMail(EmailRequestDTO emailRequestDto)
        {
            try
            {
                // Validate the DTO (optional, but can help ensure the data is correct)
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(emailRequestDto, new ValidationContext(emailRequestDto), validationResults, true);

                if (!isValid)
                {
                    // If validation fails, return a meaningful error message
                    return "Invalid email request data: " + string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                }

                // Attempt to send the email
                 _emailService.SendEmailInBackground(emailRequestDto.Email, emailRequestDto.Subject, emailRequestDto.Body);

                // If sending email succeeds, return success message
                return "Email sent successfully.";
            }
            catch (ArgumentNullException ex)
            {
                // Handle specific error if needed (e.g., email or subject is null)
                return "Error: Missing required information. " + ex.Message;
            }
            catch (FormatException ex)
            {
                // Handle invalid format (e.g., email format issue)
                return "Error: Invalid email format. " + ex.Message;
            }
            catch (Exception ex)
            {
                // Catch any other unexpected exceptions
                return "An unexpected error occurred while sending the email: " + ex.Message;
            }
        }


    }
}
