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
                    FailedLoginAttempts = student.FailedLoginAttempts,
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
                FailedLoginAttempts = student.FailedLoginAttempts,
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

            // Ensure Student Repository is initialized
            if (_studentRepository == null)
            {
                throw new InvalidOperationException("Student repository is not initialized.");
            }

            // Save the student first
            await _studentRepository.AddAsync(student);

            // Add social media links after the student is saved
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

            //return "Student Updated Succesfuly";



            //await _emailService.SendEmailAsync(student.Email, "Profile Updated", $"{student.FirstName} {student.LastName}, your profile has been successfully updated.");
            return "Student profile update successful";

        }




        public async Task DeleteStudentAsync(string nic)
        {
            if (string.IsNullOrWhiteSpace(nic))
            {
                throw new ArgumentException("NIC cannot be null or empty.", nameof(nic));
            }

            // Fetch the student record
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
            {
                throw new Exception($"Student with NIC {nic} not found.");
            }

            // Delete the image if it exists
            if (!string.IsNullOrEmpty(student.ImagePath))
            {
                _imageService.DeleteImage(student.ImagePath);
            }

            // Delete related data
            //await _userService.DeleteAsync(nic);
            //await _socialMediaLinksService.DeleteAsync(nic);

            // Delete the student record
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
                    student.FailedLoginAttempts = 0;
                    await _studentRepository.UpdateStudentAccount(student);

                    //await _emailService.SendEmailAsync(student.Email, "Account Unlocked",
                    //$"Dear {student.FirstName} {student.LastName},\n\n" +
                    //"your account has been unlocked.Please login with your password and you can continue your studies");


                    return "Account has been unlocked.";
                }
            }


        }

        //////Student profile get
        public async Task<StudentResponseDto> GetStudentProfileByNICAsync(string nic)
        {
            var student = await _studentRepository.GetStudentProfileByNICAsync(nic);

            if (student == null)
            {
                throw new KeyNotFoundException("Student not found");
            }

            // Mapping to DTO
            var studentResponse = new StudentResponseDto
            {
                NIC = student.NIC,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Phone = student.Phone,
                IsLocked = student.IsLocked,
                FailedLoginAttempts = student.FailedLoginAttempts,
                ImagePath = student.ImagePath,
                Address = new AddressResponseDto
                {
                    AddressLine1 = student.Address.AddressLine1,
                    AddressLine2 = student.Address.AddressLine2,
                    City = student.Address.City,
                    State = student.Address.State,
                    PostalCode = student.Address.PostalCode,
                    Country = student.Address.Country
                }
            };

            // Add Social Media Links
            if (student.SocialMediaLinks != null)
            {
                studentResponse.SocialMediaLinks = new SocialMediaLinksResponseDto
                {
                    LinkedIn = student.SocialMediaLinks.LinkedIn,
                    Instagram = student.SocialMediaLinks.Instagram,
                    Facebook = student.SocialMediaLinks.Facebook,
                    GitHub = student.SocialMediaLinks.GitHub,
                    WhatsApp = student.SocialMediaLinks.WhatsApp,
                    StudentNIC = student.NIC
                };
            }

            // Add Enrollment and Course Data
            studentResponse.Enrollments = student.Enrollment.Select(e => new EnrollmentResponseDto
            {
                Id = e.Id,
                EnrollmentDate = e.EnrollmentDate,
                PaymentPlan = e.PaymentPlan,
                IsComplete = e.IsComplete,
                StudentNIC = e.StudentNIC,
                CourseId = e.CourseId,
                course = new CourseResponseDTO
                {
                    Id = e.Course.Id,
                    CourseName = e.Course.CourseName,
                    Level = e.Course.Level,
                    Duration = e.Course.Duration,
                    Fees = e.Course.Fees,
                    ImagePaths = e.Course.ImagePaths != null
                                  ? e.Course.ImagePaths.Split(',').ToList()
            : new List<string>(),
                    Description = e.Course.Description
                },
                payments = e.payments.Select(p => new PaymentResponseDto
                {
                    Id = p.Id,
                    TotalPaidAmount = p.TotalPaidAmount,
                    Amount = p.Amount,
                    DueAmount = p.DueAmount,
                    PaymentDate = p.PaymentDate,
                    EnrollmentId = (Guid)p.EnrollmentId
                }).FirstOrDefault() // Assuming one payment per enrollment, adjust if needed
            }).ToList();

            return studentResponse;
        }


        public async Task<StudentResponseDto> UpdateStudentAsync(string nic, StudentUpdateRequestDto updateDto)
        {
            var student = await _studentRepository.GetByNicAsync(nic);
            if (student == null)
            {
                throw new Exception($"Student with NIC {nic} not found.");
            }

            // Manually map properties from StudentUpdateRequestDto to Student entity
            student.FirstName = updateDto.FirstName;
            student.LastName = updateDto.LastName;
            student.Email = updateDto.Email;
            student.Phone = updateDto.Phone;

            // Manually map Address fields
            student.Address.AddressLine1 = updateDto.Address.AddressLine1;
            student.Address.AddressLine2 = updateDto.Address.AddressLine2;
            student.Address.City = updateDto.Address.City;
            student.Address.State = updateDto.Address.State;
            student.Address.PostalCode = updateDto.Address.PostalCode;
            student.Address.Country = updateDto.Address.Country;

            // Update the student in the repository
            _studentRepository.Update(student);
            await _studentRepository.SaveAsync();

            // Manually map Student entity to StudentResponseDto
            var responseDto = new StudentResponseDto
            {
                NIC = student.NIC,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Phone = student.Phone,
                IsLocked = student.IsLocked,
                FailedLoginAttempts = student.FailedLoginAttempts,
                ImagePath = student.ImagePath,
                Address = new AddressResponseDto
                {
                    AddressLine1 = student.Address.AddressLine1,
                    AddressLine2 = student.Address.AddressLine2,
                    City = student.Address.City,
                    State = student.Address.State,
                    PostalCode = student.Address.PostalCode,
                    Country = student.Address.Country
                }
            };

            return responseDto;
        }

    }
}
