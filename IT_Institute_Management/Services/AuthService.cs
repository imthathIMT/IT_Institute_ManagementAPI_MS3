using IT_Institute_Management.Database;
using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.EmailSection.Models;
using IT_Institute_Management.EmailSection.Service;
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
    private readonly sendmailService _sendmailService;

    private const int MaxLoginAttempts = 5;

    public AuthService(IAuthRepository authRepository, IPasswordHasher passwordHasher, IConfiguration configuration, IStudentRepository studentRepository, InstituteDbContext context, sendmailService sendmailService)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _studentRepository = studentRepository;
        _context = context;
        _sendmailService = sendmailService;
    }

    public async Task<string> GetLoginUserToken(UserLoginModal request)
    {
        var user = await _authRepository.GetLoginUser(request.nic);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

       
        if (user.Role == Role.Student)
        {
            var student = await _studentRepository.GetByNicAsync(user.NIC);
            if (student != null)
            {
                if (student.IsLocked)
                {
                    throw new Exception("Your account is locked. Please contact admin.");
                }

                if (!_passwordHasher.VerifyHashedPassword(user.Password, request.Password))
                {
                    student.FailedLoginAttempts++;
                    if (student.FailedLoginAttempts >= MaxLoginAttempts)
                    {
                        student.IsLocked = true;
                    }
                    else
                    {
                        int remainingAttempts = MaxLoginAttempts - student.FailedLoginAttempts;
                        await _context.SaveChangesAsync(); 
                        throw new Exception($"Incorrect password. Dear student you have {remainingAttempts} login attempt(s) remaining.");
                    }

                    await _context.SaveChangesAsync();

                    var sendMailRequest = new SendMailRequest
                    {
                        NIC = student.NIC,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Email = student.Email,
                        TemplateName = "AccountLockedFailedLogin"

                    };

                    if (_sendmailService == null)
                    {
                        throw new InvalidOperationException("_sendmailService is not initialized.");
                    }

                  
                    await _sendmailService.Sendmail(sendMailRequest).ConfigureAwait(false);
                    throw new Exception("Your account has been locked due to too many failed login attempts.");
                }

             
                student.FailedLoginAttempts = 0;
                await _context.SaveChangesAsync();

                var token = CreateToken(user);
                return token;
            }
        }

      
        if (_passwordHasher.VerifyHashedPassword(user.Password, request.Password))
        {
            var token = CreateToken(user);
            return token;
        }
        else
        {
            throw new Exception("Incorrect nic or password.");
        }
    }


    private string CreateToken(User user)
    {
        var claimsList = new List<Claim>
        {
            new Claim("nic", user.NIC.ToString()),
            new Claim("Role", user.Role.ToString()),
            new Claim(ClaimTypes.Role,user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims: claimsList,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
