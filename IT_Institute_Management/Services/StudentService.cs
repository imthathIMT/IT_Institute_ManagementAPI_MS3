using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.EmailSerivice;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace IT_Institute_Management.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEmailService _emailService;

        public StudentService(IStudentRepository studentRepository, IEmailService emailService)
        {
            _studentRepository = studentRepository;
            _emailService = emailService;
        }

        public async Task<List<StudentResponseDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Select(student => new StudentResponseDto
            {
                NIC = student.NIC,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Phone = student.Phone,
                WhatsappNumber = student.WhatsappNuber,
                Status = student.Status,
                ImagePath = student.ImagePath,
                Address = new AddressResponseDto
                {
                    AddressLine1 = student.Address?.AddressLine1,
                    AddressLine2 = student.Address?.AddressLine2,
                    City = student.Address?.City,
                    State = student.Address?.State,
                    ZipCode = student.Address?.ZipCode,
                    Country = student.Address?.Country
                }
            }).ToList();
        }

        public async Task<StudentResponseDto> GetStudentByNicAsync(string nic)
        {
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
            {
                throw new ApplicationException($"Student with NIC {nic} not found.");
            }

            return new StudentResponseDto
            {
                NIC = student.NIC,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Phone = student.Phone,
                WhatsappNumber = student.WhatsappNuber,
                Status = student.Status,
                ImagePath = student.ImagePath,
                Address = new AddressResponseDto
                {
                    AddressLine1 = student.Address?.AddressLine1,
                    AddressLine2 = student.Address?.AddressLine2,
                    City = student.Address?.City,
                    State = student.Address?.State,
                    ZipCode = student.Address?.ZipCode,
                    Country = student.Address?.Country
                }
            };
        }

        public async Task AddStudentAsync(StudentRequestDto studentDto)
        {
            var student = new Student
            {
                NIC = studentDto.NIC,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                Phone = studentDto.Phone,
                WhatsappNuber = studentDto.WhatsappNumber,
                Password = studentDto.Password,
                ImagePath = studentDto.ImagePath,
                Status = true, // Account is active when created
                Address = new Address
                {
                    AddressLine1 = studentDto.Address.AddressLine1,
                    AddressLine2 = studentDto.Address.AddressLine2,
                    City = studentDto.Address.City,
                    State = studentDto.Address.State,
                    ZipCode = studentDto.Address.ZipCode,
                    Country = studentDto.Address.Country
                }
            };

            await _studentRepository.AddAsync(student);

            // Send email after registration
            await _emailService.SendEmailAsync(student.Email, "Registration Successful", $"{student.FirstName} {student.LastName}, your registration was successful.");
        }

        public async Task UpdateStudentAsync(string nic, StudentRequestDto studentDto)
        {
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
            {
                throw new ApplicationException($"Student with NIC {nic} not found.");
            }

            student.NIC = studentDto.NIC;
            student.FirstName = studentDto.FirstName;
            student.LastName = studentDto.LastName;
            student.Email = studentDto.Email;
            student.Phone = studentDto.Phone;
            student.WhatsappNuber = studentDto.WhatsappNumber;
            student.Password = studentDto.Password;
            student.ImagePath = studentDto.ImagePath;
            student.Address = new Address
            {
                AddressLine1 = studentDto.Address.AddressLine1,
                AddressLine2 = studentDto.Address.AddressLine2,
                City = studentDto.Address.City,
                State = studentDto.Address.State,
                ZipCode = studentDto.Address.ZipCode,
                Country = studentDto.Address.Country
            };

            await _studentRepository.UpdateAsync(student);

            // Send email after update
            await _emailService.SendEmailAsync(student.Email, "Profile Updated", $"{student.FirstName} {student.LastName}, your profile has been successfully updated.");
        }

        public async Task DeleteStudentAsync(string nic)
        {
            await _studentRepository.DeleteAsync(nic);
        }

    }
}
