using IT_Institute_Management.PasswordService;
using Microsoft.AspNetCore.Identity;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher;

    public PasswordHasher()
    {
        _passwordHasher = new PasswordHasher<object>();
    }

   
    public string HashPassword(string password)
    {
       
        return _passwordHasher.HashPassword(null, password);
    }

   
    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        
        var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);

     
        return result == PasswordVerificationResult.Success;
    }
}
