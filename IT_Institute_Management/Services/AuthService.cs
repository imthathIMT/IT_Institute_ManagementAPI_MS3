using IT_Institute_Management.Database;
using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using IT_Institute_Management.PasswordService;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly IStudentRepository _studentRepository;
    private readonly InstituteDbContext _context;

    private const int MaxLoginAttempts = 5;

    public AuthService(IAuthRepository authRepository, IPasswordHasher passwordHasher, IConfiguration configuration, IStudentRepository studentRepository, InstituteDbContext context)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _studentRepository = studentRepository;
        _context = context;
    }

    public async Task<string> GetLoginUserToken(UserLoginModal request)
    {
        var user = await _authRepository.GetLoginUser(request.nic);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        // Check if the user is a student and handle login attempts
        if (user.Role == Role.Student)
        {
            var student = await _studentRepository.GetByNicAsync(user.NIC);
            if (student != null)
            {
                if (student.IsLocked)
                {
                    throw new Exception("Your account is locked. Please contact admin");
                }

                if (!_passwordHasher.VerifyHashedPassword(user.Password, request.Password))
                {
                    student.FailedLoginAttempts++;
                    if (student.FailedLoginAttempts >= MaxLoginAttempts)
                    {
                        student.IsLocked = true;
                    }
                    await _context.SaveChangesAsync(); // Save only once after updating failed attempts or locking
                    throw new Exception(student.IsLocked ?
                        "Your account has been locked due to too many failed login attempts." : "Incorrect password.");
                }

                // Reset failed attempts if login is successful
                student.FailedLoginAttempts = 0;
                await _context.SaveChangesAsync();

                var token = CreateToken(user);
                return token;
            }
        }

        // If the password is valid for any user, create the token
        if (_passwordHasher.VerifyHashedPassword(user.Password, request.Password))
        {
            var token = CreateToken(user);
            return token;
        }
        else
        {
            throw new Exception("Incorrect password.");
        }
    }


    private string CreateToken(User user)
    {
        var claimsList = new List<Claim>
        {
            new Claim("nic", user.NIC.ToString()),
            new Claim("Role", user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims: claimsList,
            expires: DateTime.Now.AddDays(10),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
