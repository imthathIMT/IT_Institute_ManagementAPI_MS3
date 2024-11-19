using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;

namespace IT_Institute_Management.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserResponseDto
            {
                NIC = u.NIC,
                Role = u.Role.ToString()
            });
        }

        public async Task<UserResponseDto> GetByIdAsync(string nic)
        {
            var user = await _userRepository.GetByIdAsync(nic);
            if (user == null) throw new Exception($"User with NIC {nic} not found.");
            return new UserResponseDto
            {
                NIC = user.NIC,
                Role = user.Role.ToString()
            };
        }

        public async Task AddAsync(UserRequestDto userDto, Role role)
        {
           
        }



        public async Task UpdateAsync(string nic, UserRequestDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(nic);
            if (user == null) throw new Exception($"User with NIC {nic} not found.");

            user.Password = userDto.Password;
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string nic)
        {
            var user = await _userRepository.GetByIdAsync(nic);
            if (user == null) throw new Exception($"User with NIC {nic} not found.");

            await _userRepository.DeleteAsync(nic);
        }
    }
}
