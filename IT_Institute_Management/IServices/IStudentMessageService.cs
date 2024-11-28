
using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;

namespace IT_Institute_Management.IServices
{
    public interface IStudentMessageService
    {
        Task<IEnumerable<StudentMessageResponseDto>> GetAllMessagesAsync();
        Task<IEnumerable<StudentMessageResponseDto>> GetMessagesByStudentNICAsync(string studentNIC);
        Task<StudentMessageResponseDto> AddMessageAsync(StudentMessageRequestDto requestDto);
    }
}
