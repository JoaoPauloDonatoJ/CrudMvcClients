using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IClientService
    {
        //Task<IEnumerable<Client>> GetAll();
        Task<IEnumerable<ClientReponseDto>> GetAll();
        //Task<Client> GetById(int id);
        Task<ServiceResult<ClientReponseDto>> GetById(int id);
        //Task<ServiceResult<Client>> Create(Client client);
        Task<ServiceResult<ClientReponseDto>> Create(ClientCreateDto clientCreateDto);
        //Task<ServiceResult<Client>> Update(Client client);
        Task<ServiceResult<ClientReponseDto>> Update(ClientUpdateDto client);
        Task<Client> Delete(int id);
        Task<bool> EmailExist(string email);
    }
}
