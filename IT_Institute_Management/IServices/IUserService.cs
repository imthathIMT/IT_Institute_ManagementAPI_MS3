using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto> GetByIdAsync(string nic);
        Task AddAsync(UserRequestDto userDto, Role role);
        Task UpdateAsync(string nic, UserRequestDto userDto);
        Task DeleteAsync(string nic);

        Task<UserLoginModal> loginUserGet(string nic);
    }
}
