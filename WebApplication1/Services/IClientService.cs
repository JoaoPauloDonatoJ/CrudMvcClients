using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAll();
        Task<Client> GetById(int id);
        Task<ServiceResult<Client>> Create(Client client);
        Task<ServiceResult<Client>> Update(Client client);
        Task<Client> Delete(int id);
        Task<bool> EmailExist(string email);
    }
}
