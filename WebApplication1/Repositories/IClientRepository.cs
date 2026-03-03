using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAll();
        Task<Client?> GetById(int id);
        Task<Client> Add(Client client);
        Task<Client> Update(Client client);
        Task<Client> Remove(int id);
        Task<bool> EmailExist(string email);
        Task SaveChangesAsync();
    }
}
