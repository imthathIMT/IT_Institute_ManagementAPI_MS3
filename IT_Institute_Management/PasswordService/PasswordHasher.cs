using IT_Institute_Management.PasswordService;
using Microsoft.AspNetCore.Identity;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher;

    public PasswordHasher()
    {
        _passwordHasher = new PasswordHasher<object>();
    }

    // Hash password using ASP.NET Core Identity's PasswordHasher
    public string HashPassword(string password)
    {
        // The object type here is just a placeholder; ASP.NET Core Identity requires a user object
        // but we don't need to tie this to a specific user for just hashing.
        return _passwordHasher.HashPassword(null, password);
    }

    // Verify hashed password using ASP.NET Core Identity's PasswordHasher
    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        // The object type here is just a placeholder; we don't need a specific user object
        // when verifying the password.
        var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);

        // Return whether the password verification succeeded or not
        return result == PasswordVerificationResult.Success;
    }
}
