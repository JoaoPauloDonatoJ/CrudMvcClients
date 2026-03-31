using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmail(string email);

        Task<User?> Remove(int id);
    }
}
