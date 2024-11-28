using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using SendGrid.Helpers.Errors.Model;

namespace IT_Institute_Management.Services
{
    public class StudentMessageService : IStudentMessageService
    {
        private readonly IStudentMessageRepository _repository;

        public StudentMessageService(IStudentMessageRepository studentMessageRepository)
        {
            _repository = studentMessageRepository;
        }

        public async Task<IEnumerable<StudentMessageResponseDto>> GetAllMessagesAsync()
        {
            var messages = await _repository.GetAllAsync();
            if (messages == null)
            {
                throw new NotFoundException("Student messages not found");
            }
            var response = messages.Select(message => new StudentMessageResponseDto
            {
                Id = message.Id,
                Message = message.Message,
                Date = message.Date,
                StudentNIC = message.StudentNIC,
                Student = new StudentResponseDto
                {
                    NIC = message.Student.NIC,
                    FirstName = message.Student.FirstName,
                    LastName = message.Student.LastName,
                    Email = message.Student.Email,
                    Phone = message.Student.Phone,
                    IsLocked = message.Student.IsLocked,
                    FailedLoginAttempts = message.Student.FailedLoginAttempts,
                    ImagePath = message.Student.ImagePath,
                    Address = new AddressResponseDto
                    {
                        AddressLine1 = message.Student.Address.AddressLine1,
                        AddressLine2 = message.Student.Address.AddressLine2,
                        City = message.Student.Address.City,
                        State = message.Student.Address.State,
                        PostalCode = message.Student.Address.PostalCode,
                        Country = message.Student.Address.Country
                    }
                }
            });

            return response;
        }

        public async Task<IEnumerable<StudentMessageResponseDto>> GetMessagesByStudentNICAsync(string studentNIC)
        {
            var messages = await _repository.GetByStudentNICAsync(studentNIC);

            if (messages == null)
            {
                throw new NotFoundException("Student messages not found");
            }
            return messages.Select(message => new StudentMessageResponseDto
            {
                Id = message.Id,
                Message = message.Message,
                Date = message.Date,
                StudentNIC = message.StudentNIC,
                Student = new StudentResponseDto
                {
                    NIC = message.Student.NIC,
                    FirstName = message.Student.FirstName,
                    LastName = message.Student.LastName,
                    Email = message.Student.Email,
                    Phone = message.Student.Phone,
                    IsLocked = message.Student.IsLocked,
                    FailedLoginAttempts = message.Student.FailedLoginAttempts,
                    ImagePath = message.Student.ImagePath,
                    Address = new AddressResponseDto
                    {
                        AddressLine1 = message.Student.Address.AddressLine1,
                        AddressLine2 = message.Student.Address.AddressLine2,
                        City = message.Student.Address.City,
                        State = message.Student.Address.State,
                        PostalCode = message.Student.Address.PostalCode,
                        Country = message.Student.Address.Country
                    }
                }
            });
        }


    }
}
