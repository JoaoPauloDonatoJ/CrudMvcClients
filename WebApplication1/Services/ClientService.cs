using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Client>> GetAll()
            => await _repository.GetAll();

        public async Task<Client> GetById(int id)
        {
            var client = await _repository.GetById(id);

            if (client == null)
                throw new KeyNotFoundException("Cliente não localizado");

            return client;

        }

        public async Task<Client> Create(Client client)
        {
            client.Ativo = true;

            var emailExist = await _repository.EmailExist(client.Email);

            if (emailExist)
            {
                throw new InvalidOperationException("Email já cadastrado");
            }

            client.DataCadastro = DateTime.Now;

            return await _repository.Add(client);
        }

        public async Task<Client> Update(Client client)
        {
            return await _repository.Update(client);
        }

        public async Task<Client> Delete(int id)
        {
            return await _repository.Remove(id);
        }

        public async Task<bool> EmailExist(string email)
        {
            return await _repository.EmailExist(email);
        }
    }
}
