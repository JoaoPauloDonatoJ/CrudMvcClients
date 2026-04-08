using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IProfileRepository
    {
        Task <List<Profile>> GetAllProfileAsync();
    }
}
