using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAll();
        Task<Client> GetById(int id);
        Task<Client> Create(Client client);
        Task<Client> Update(Client client);
        Task<Client> Delete(int id);
        Task<bool> EmailExist(string email);
    }
}
