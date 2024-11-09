using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface IStudentService
    {
        Task<List<StudentResponseDto>> GetAllStudentsAsync();
        Task<StudentResponseDto> GetStudentByNicAsync(string nic);
        Task AddStudentAsync(StudentRequestDto studentDto);
        Task<string> UpdateStudentAsync(string nic, StudentRequestDto studentDto);
        Task DeleteStudentAsync(string nic);
        Task UpdatePasswordAsync(string nic, UpdatePasswordRequestDto updatePasswordDto);
    }
}
