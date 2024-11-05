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

            var studentResponse = new List<StudentResponseDto>();


            foreach (var student in students)
            {
                var response = new StudentResponseDto();

                response.NIC = student.NIC;
                response.FirstName = student.FirstName;
                response.LastName = student.LastName;
                response.Email = student.Email;
                response.Phone = student.Phone;
                response.WhatsappNumber = student.WhatsappNuber;
                response.Status = student.Status;
                response.ImagePath = student.ImagePath;
                response.Address = new AddressResponseDto
                {
                    AddressLine1 = student.Address?.AddressLine1,
                    AddressLine2 = student.Address?.AddressLine2,
                    City = student.Address?.City,
                    State = student.Address?.State,
                    ZipCode = student.Address?.ZipCode,
                    Country = student.Address?.Country
                };            
               studentResponse.Add(response);
            }
            return studentResponse;
           
        }

    }
}
