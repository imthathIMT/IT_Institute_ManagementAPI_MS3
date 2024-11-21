using Microsoft.CodeAnalysis.Scripting;

namespace IT_Institute_Management.PasswordService
{
    public class PasswordHasher:IPasswordHasher
    {
        // Hash password using BCrypt
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verify hashed password using BCrypt
        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}
