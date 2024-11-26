using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.EmailSerivice;
using IT_Institute_Management.Entity;
using IT_Institute_Management.ImageService;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using IT_Institute_Management.PasswordService;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Protocol.Core.Types;

namespace IT_Institute_Management.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        private readonly ISocialMediaLinksService _socialMediaLinksService;


        public StudentService(IStudentRepository studentRepository, IEmailService emailService, IPasswordHasher passwordHasher, IImageService imageService, IUserService userService, ISocialMediaLinksService socialMediaLinksService)
        {
            _studentRepository = studentRepository;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
            _imageService = imageService;
            _userService = userService;
            _socialMediaLinksService = socialMediaLinksService;
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
            if (studentDto == null)
            {
                throw new ArgumentNullException(nameof(studentDto), "StudentDto cannot be null.");
            }

            if (studentDto.Address == null)
            {
                throw new ArgumentNullException(nameof(studentDto.Address), "Address cannot be null.");
            }

            var imagePath = string.Empty;

            if (studentDto.Image != null)
            {
                if (_imageService == null)
                {
                    throw new InvalidOperationException("Image service is not initialized.");
                }

                imagePath = await _imageService.SaveImage(studentDto.Image, "students");
            }

            if (_passwordHasher == null)
            {
                throw new InvalidOperationException("Password hasher is not initialized.");
            }

            var hashedPassword = _passwordHasher.HashPassword(studentDto.Password);

            var student = new Student
            {
                NIC = studentDto.NIC,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                Phone = studentDto.Phone,
                Password = hashedPassword,
                ImagePath = imagePath,
                FailedLoginAttempts = 0,
                IsLocked = false,
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

            // Check for social media service
            if (_socialMediaLinksService == null)
            {
                throw new InvalidOperationException("SocialMediaLinks service not initialized.");
            }

            await _socialMediaLinksService.CreateAsync(new SocialMediaLinksRequestDto
            {
                StudentNIC = studentDto.NIC,
            });

            // Ensure UserService is initialized
            if (_userService == null)
            {
                throw new InvalidOperationException("User service is not initialized.");
            }

            // Add the user
            await _userService.AddAsync(new UserRequestDto
            {
                NIC = studentDto.NIC,
                Password = hashedPassword
            }, Role.Student);

            // Ensure Student Repository is initialized
            if (_studentRepository == null)
            {
                throw new InvalidOperationException("Student repository is not initialized.");
            }

            await _studentRepository.AddAsync(student);

            // Check Email Service initialization
            if (_emailService == null)
            {
                throw new InvalidOperationException("Email service is not initialized.");
            }

            // Uncomment the email service once setup is correct
            // await _emailService.SendEmailAsync(student.Email, "Student Registration", $"Welcome {student.FirstName} {student.LastName}, your registration was successful.");
        }




        public async Task<string> UpdateStudentAsync(string nic, StudentRequestDto studentDto)
        {
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
            {
                throw new Exception($"Student with NIC {nic} not found.");
            }


            if (studentDto.Image != null)
            {

                if (!string.IsNullOrEmpty(student.ImagePath))
                {
                    _imageService.DeleteImage(student.ImagePath);
                }


                student.ImagePath = await _imageService.SaveImage(studentDto.Image, "students");
            }

            student.NIC = studentDto.NIC;
            student.FirstName = studentDto.FirstName;
            student.LastName = studentDto.LastName;
            student.Email = studentDto.Email;
            student.Phone = studentDto.Phone;
            student.Address = new Address
            {
                AddressLine1 = studentDto.Address.AddressLine1,
                AddressLine2 = studentDto.Address.AddressLine2,
                City = studentDto.Address.City,
                State = studentDto.Address.State,
                PostalCode = studentDto.Address.PostalCode,
                Country = studentDto.Address.Country
            };


            if (!string.IsNullOrEmpty(studentDto.Password))
            {
                student.Password = _passwordHasher.HashPassword(studentDto.Password);

                await _userService.UpdateAsync(nic, new UserRequestDto { Password = studentDto.Password });
            }

            await _studentRepository.UpdateAsync(student);

            return "Student Updated Succesfuly";

        }




        public async Task DeleteStudentAsync(string nic)
        {
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
            {
                throw new Exception($"Student with NIC {nic} not found.");
            }


            if (!string.IsNullOrEmpty(student.ImagePath))
            {
                _imageService.DeleteImage(student.ImagePath);
            }


            await _userService.DeleteAsync(nic);

            await _studentRepository.DeleteAsync(nic);
        }


        public async Task UpdatePasswordAsync(string nic, UpdatePasswordRequestDto updatePasswordDto)
        {
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
            {
                throw new Exception($"Student with NIC {nic} not found.");
            }


            if (!_passwordHasher.VerifyHashedPassword(student.Password, updatePasswordDto.CurrentPassword))
            {
                throw new Exception("Current password is incorrect.");
            }


            var hashedPassword = _passwordHasher.HashPassword(updatePasswordDto.NewPassword);


            student.Password = hashedPassword;

            await _userService.UpdateAsync(nic, new UserRequestDto { Password = hashedPassword });
            await _studentRepository.UpdateAsync(student);


            //await _emailService.SendEmailAsync(student.Email, "Password Updated", $"{student.FirstName} {student.LastName}, your password has been successfully updated.");
        }


        public async Task<string> LockAccountAsync(string nic)
        {
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
            {
                throw new Exception("Student not found.");
            }
            else
            {
                student.IsLocked = true;
                await _studentRepository.UpdateStudentAccount(student);

                //await _emailService.SendEmailAsync(student.Email, "Account Locked",
                // $"Dear {student.FirstName} {student.LastName},\n\n" +
                // "your account has been locked by admin. please contact admin");

                return "Account has been locked.";
            }
        }


        public async Task<string> UnlockAccountAsync(UnlockAccountDto unlockDto)
        {
            var student = await _studentRepository.GetByNicAsync(unlockDto.NIC);
            if (student == null)
            {
                throw new Exception("Student not found.");
            }
            else
            {
                var hashedPassword = _passwordHasher.HashPassword(unlockDto.NewPassword);

                await _userService.UpdateAsync(unlockDto.NIC, new UserRequestDto { Password = hashedPassword });

                student.IsLocked = false;
                student.Password = hashedPassword;
                student.FailedLoginAttempts = 0;


                await _studentRepository.UpdateStudentAccount(student);

                // Send unlock account email
                //await _emailService.SendEmailAsync(student.Email, "Account Unlocked",
                //    $"Dear {student.FirstName} {student.LastName},\n\n" +
                //    "Your account has been unlocked. Your password has been reset. Please login with your new password.");

                return "Account has been unlocked and password updated.";
            }


        }

        public async Task<string> DirectUnlock(string nic)
        {
            if (nic == null)
            {
                throw new Exception("NIC is required");
            }
            else
            {
                var student = await _studentRepository.GetByNicAsync(nic);
                if (student == null)
                {
                    throw new Exception("Student not found.");
                }
                else
                {
                    student.IsLocked = false;
                    await _studentRepository.UpdateStudentAccount(student);

                    //await _emailService.SendEmailAsync(student.Email, "Account Unlocked",
                    //$"Dear {student.FirstName} {student.LastName},\n\n" +
                    //"your account has been unlocked.Please login with your password and you can continue your studies");


                    return "Account has been unlocked.";
                }
            }


        }

    }
}
