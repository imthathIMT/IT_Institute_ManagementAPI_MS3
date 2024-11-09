using Microsoft.CodeAnalysis.Scripting;

namespace IT_Institute_Management.PasswordService
{
    public class PasswordHasher:IPasswordHasher
    {
        public string HashPassword(string password)
        {
            // hashes the password
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verifies the password by comparing the provided password with the hashed password
        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            // Compares the provided password with the hashed password
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}
