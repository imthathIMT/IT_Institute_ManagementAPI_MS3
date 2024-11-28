using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;

namespace IT_Institute_Management.Services
{
    public class StudentMessageService : IStudentMessageService
    {
        private readonly IStudentMessageRepository _repository;

        public StudentMessageService(IStudentMessageRepository studentMessageRepository)
        {
            _repository = studentMessageRepository;
        }
    }
}
