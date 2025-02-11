﻿using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.EmailSection.Models;
using IT_Institute_Management.EmailSection.Service;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IContactUsRepository _contactUsRepository;
        private readonly sendmailService _sendmailService;

        public ContactUsService(IContactUsRepository contactUsRepository, sendmailService sendmailService)
        {
            _contactUsRepository = contactUsRepository;
            _sendmailService = sendmailService;
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
            var sendMailRequest = new SendMailRequest
            {
                FirstName = contactUsDto.Name,
                Email = contactUsDto.Email,
                TemplateName = "EnquiryResponse"

            };

            if (_sendmailService == null)
            {
                throw new InvalidOperationException("_sendmailService is not initialized.");
            }

          
            await _sendmailService.Sendmail(sendMailRequest).ConfigureAwait(false);
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
                
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(emailRequestDto, new ValidationContext(emailRequestDto), validationResults, true);

                if (!isValid)
                {
                  
                    return "Invalid email request data: " + string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                }

                var enquiry = await _contactUsRepository.GetByEmail(emailRequestDto.Email);

              
                var sendMailRequest = new SendMailRequest
                {
                    FirstName = enquiry.Name,
                    Email = enquiry.Email,
                    AdminMessage = emailRequestDto.Body,
                    TemplateName = "AdminResponse"

                };

                if (_sendmailService == null)
                {
                    throw new InvalidOperationException("_sendmailService is not initialized.");
                }

              
                await _sendmailService.Sendmail(sendMailRequest).ConfigureAwait(false);
               
                return "Email sent successfully.";
            }
            catch (ArgumentNullException ex)
            {
               
                return "Error: Missing required information. " + ex.Message;
            }
            catch (FormatException ex)
            {
               
                return "Error: Invalid email format. " + ex.Message;
            }
            catch (Exception ex)
            {
                
                return "An unexpected error occurred while sending the email: " + ex.Message;
            }
        }


    }
}
