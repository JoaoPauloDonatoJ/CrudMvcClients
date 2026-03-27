using BCrypt.Net;

namespace WebApplication1.Services
{
    public class PasswordService : IPasswordService
    {
        public PasswordService(){}

        public async Task<string> HashPassword(string password)
        {
            //return await _passwordService.HashPassword(password);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;
        }

        public async Task<bool> VerifyPassword(string password, string hash)
        {
            //return await _passwordService.VerifyPassword(password, hash);
            bool result = BCrypt.Net.BCrypt.Verify(password, hash);
            return result;
        }
    }
}
