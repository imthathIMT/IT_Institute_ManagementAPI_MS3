using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface IStudentService
    {
        Task<List<StudentResponseDto>> GetAllStudentsAsync();
    }
}
