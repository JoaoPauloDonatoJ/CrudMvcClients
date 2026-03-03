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

        public async Task<ServiceResult<Client>> Create(Client client)
        {
            try
            {
                var emailExist = await _repository.EmailExist(client.Email);

                if (emailExist)
                {
                    throw new InvalidOperationException("Email já cadastrado");
                }

                client.Ativo = true;
                client.DataCadastro = DateTime.Now;

                 await _repository.Add(client);
                return ServiceResult<Client>.Ok(client);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro na criação do cliente: {ex.Message}", ex);
            }
        }

        public async Task<ServiceResult<Client>> Update(Client client)
        {
            try
            {
                var existingClient = await _repository.GetById(client.Id);

                if (existingClient == null)
                {
                    throw new KeyNotFoundException("Cliente não localizado");
                }

                //Comparando os dados, se forem identicos ele marca como sem alterações
                if (existingClient.Nome == client.Nome && 
                    existingClient.Email == client.Email && 
                    existingClient.Ativo == client.Ativo)
                {
                    return ServiceResult<Client>.NoChanges(existingClient);
                }

                // 3. ATUALIZA as propriedades do objeto rastreado com os novos valores
                existingClient.Nome = client.Nome;
                existingClient.Email = client.Email;
                existingClient.Ativo = client.Ativo;

                await _repository.SaveChangesAsync();
                return ServiceResult<Client>.Ok(existingClient);

            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro na atualização do cliente: {ex.Message}", ex);
            }
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
