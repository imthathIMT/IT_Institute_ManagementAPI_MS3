using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.EmailSerivice;
using IT_Institute_Management.Entity;
using IT_Institute_Management.ImageService;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using IT_Institute_Management.PasswordService;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace IT_Institute_Management.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;


        public StudentService(IStudentRepository studentRepository, IEmailService emailService, IPasswordHasher passwordHasher, IImageService imageService, IUserService userService)
        {
            _studentRepository = studentRepository;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
            _imageService = imageService;
            _userService = userService;
        }


        public async Task<List<StudentResponseDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();

            if (students == null)
            {
                throw new Exception("Students not found");
            }
            else
            {
                return students.Select(student => new StudentResponseDto
                {
                    NIC = student.NIC,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    Phone = student.Phone,
                    IsLocked = student.IsLocked,
                    ImagePath = student.ImagePath,
                    Address = new AddressResponseDto
                    {
                        AddressLine1 = student.Address?.AddressLine1,
                        AddressLine2 = student.Address?.AddressLine2,
                        City = student.Address?.City,
                        State = student.Address?.State,
                        PostalCode = student.Address?.PostalCode,
                        Country = student.Address?.Country
                    }
                }).ToList();
            }
        }

        public async Task<StudentResponseDto> GetStudentByNicAsync(string nic)
        {
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
            {
                throw new Exception($"Student with NIC {nic} not found.");
            }

            return new StudentResponseDto
            {
                NIC = student.NIC,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Phone = student.Phone,
                IsLocked = student.IsLocked,
                ImagePath = student.ImagePath,
                Address = new AddressResponseDto
                {
                    AddressLine1 = student.Address?.AddressLine1,
                    AddressLine2 = student.Address?.AddressLine2,
                    City = student.Address?.City,
                    State = student.Address?.State,
                    PostalCode = student.Address?.PostalCode,
                    Country = student.Address?.Country
                }
            };
        }

        public async Task AddStudentAsync(StudentRequestDto studentDto)
        {
            var imagePath = string.Empty;

            if (studentDto.Image != null)
            {
                // Specify the folder name as "students"
                imagePath = await _imageService.SaveImage(studentDto.Image, "students");
            }

            var student = new Student
            {
                NIC = studentDto.NIC,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                Phone = studentDto.Phone,
                Password = _passwordHasher.HashPassword(studentDto.Password), //BCrypt.Net.BCrypt.HashPassword(studentDto.Password),
                ImagePath = imagePath,  // Store image path in database
                IsLocked = true, // Account is active when created
                Address = new Address
                {
                    AddressLine1 = studentDto.Address.AddressLine1,
                    AddressLine2 = studentDto.Address.AddressLine2,
                    City = studentDto.Address.City,
                    State = studentDto.Address.State,
                    PostalCode = studentDto.Address.PostalCode,
                    Country = studentDto.Address.Country
                }
            };

            await _studentRepository.AddAsync(student);

            // Send email after registration
            await _emailService.SendEmailAsync(student.Email, "Student Registration", $"Welcome {student.FirstName} {student.LastName}, your registration was successful.");
        }


        public async Task<string> UpdateStudentAsync(string nic, StudentRequestDto studentDto)
        {
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
            {
                throw new Exception($"Student with NIC {nic} not found.");
            }

            // If an image is uploaded, save the new image and delete the old one
            if (studentDto.Image != null)
            {
                // Delete the old image
                if (!string.IsNullOrEmpty(student.ImagePath))
                {
                    _imageService.DeleteImage(student.ImagePath);
                }

                // Specify the folder name as "students"
                student.ImagePath = await _imageService.SaveImage(studentDto.Image, "students");
            }

            student.NIC = studentDto.NIC;
            student.FirstName = studentDto.FirstName;
            student.LastName = studentDto.LastName;
            student.Email = studentDto.Email;
            student.Phone = studentDto.Phone;
            student.Password = studentDto.Password;
            student.Address = new Address
            {
                AddressLine1 = studentDto.Address.AddressLine1,
                AddressLine2 = studentDto.Address.AddressLine2,
                City = studentDto.Address.City,
                State = studentDto.Address.State,
                PostalCode = studentDto.Address.PostalCode,
                Country = studentDto.Address.Country
            };

            await _studentRepository.UpdateAsync(student);

            // Send email after update
            await _emailService.SendEmailAsync(student.Email, "Profile Updated", $"{student.FirstName} {student.LastName}, your profile has been successfully updated.");
            return "Student profile update successful";
        }



        public async Task DeleteStudentAsync(string nic)
        {
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
            {
                throw new Exception($"Student with NIC {nic} not found.");
            }

            // Delete the image associated with the student
            if (!string.IsNullOrEmpty(student.ImagePath))
            {
                _imageService.DeleteImage(student.ImagePath);
            }

            await _studentRepository.DeleteAsync(nic);
        }


        public async Task UpdatePasswordAsync(string nic, UpdatePasswordRequestDto updatePasswordDto)
        {
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
            {
                throw new Exception($"Student with NIC {nic} not found.");
            }

            // Verify the current password against the stored hash
            if (!_passwordHasher.VerifyHashedPassword(student.Password, updatePasswordDto.CurrentPassword))
            {
                throw new Exception("Current password is incorrect.");
            }

            // Hash the new password before storing it
            var hashedPassword = _passwordHasher.HashPassword(updatePasswordDto.NewPassword);

            // Update the student's password
            student.Password = hashedPassword;

            await _studentRepository.UpdateAsync(student);

            // Send email after password update
            await _emailService.SendEmailAsync(student.Email, "Password Updated", $"{student.FirstName} {student.LastName}, your password has been successfully updated.");
        }

    }
}
