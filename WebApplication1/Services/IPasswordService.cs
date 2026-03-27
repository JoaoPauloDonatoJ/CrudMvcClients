using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IPasswordService
    {
        Task<string> HashPassword(string password);
        Task<bool> VerifyPassword(string password, string hash);
    }
}
