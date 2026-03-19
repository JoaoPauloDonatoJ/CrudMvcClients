using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ClientReponseDto>> GetAll()
        {
            var clients = await _repository.GetAll();

            return clients.Select(c => new ClientReponseDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Ativo = c.Ativo,
                DataCadastro = c.DataCadastro
            }).ToList();
        }


        public async Task<ServiceResult<ClientReponseDto>> GetById(int id)
        {
            var client = await _repository.GetById(id);

            if (client == null)
                return ServiceResult<ClientReponseDto>.Failure("Cliente não localizado");

            var responseDto = MapToResponse(client);


            return ServiceResult<ClientReponseDto>.Ok(responseDto);

        }

        public async Task<ServiceResult<ClientReponseDto>> Create(ClientCreateDto clientDto)
        {

            var emailExist = await _repository.EmailExist(clientDto.Email);

            if (emailExist)
            {
                return ServiceResult<ClientReponseDto>.Failure("Este e-mail já está cadastrado no sistema.");
            }

            var newUser = new User
            {
                Email = clientDto.Email,
                Nome = clientDto.Nome,
                Ativo = true,
                Password = "SenhaTemporaria",
                DataCadastro = DateTime.Now
            };

            //Mapeamento DTO -> Model (Entidade)
            var newClient = new Client
            {
                Email = clientDto.Email,
                Nome = clientDto.Nome,
                Ativo = true,
                User = newUser,
                DataCadastro = DateTime.Now
            };

            await _repository.Add(newClient);
            await _repository.SaveChangesAsync();

            // 4. Mapeamento Model -> Response Dto
            // Agora o newClient JÁ POSSUI o Id preenchido pelo banco
            var response = new ClientReponseDto
            {
                Id = newClient.Id,
                Nome = newClient.Nome,
                Email = newClient.Email,
                Ativo = newClient.Ativo,
                DataCadastro = newClient.DataCadastro
            };

            return ServiceResult<ClientReponseDto>.Ok(response);


        }

        public async Task<ServiceResult<ClientReponseDto>> Update(ClientUpdateDto clientUpdateDto)
        {

            var existingClient = await _repository.GetById(clientUpdateDto.Id);

            if (existingClient == null)
            {
                return ServiceResult<ClientReponseDto>.Failure("Cliente não localizado");
            }

            //Comparando os dados, se forem identicos ele marca como sem alterações
            if (existingClient.Nome == clientUpdateDto.Nome &&
                existingClient.Email == clientUpdateDto.Email &&
                existingClient.Ativo == clientUpdateDto.Ativo)
            {

                return ServiceResult<ClientReponseDto>.NoChanges(MapToResponse(existingClient));
            }

            // 3. ATUALIZA as propriedades do objeto rastreado com os novos valores
            existingClient.Nome = clientUpdateDto.Nome;
            existingClient.Email = clientUpdateDto.Email;
            existingClient.Ativo = clientUpdateDto.Ativo;

            // Criamos o DTO de resposta manualmente aqui
            var responseDto = new ClientReponseDto
            {
                Id = existingClient.Id,
                Nome = existingClient.Nome,
                Email = existingClient.Email,
                Ativo = existingClient.Ativo,
                DataCadastro = existingClient.DataCadastro
            };

            await _repository.SaveChangesAsync();
            return ServiceResult<ClientReponseDto>.Ok(responseDto);


        }

        public async Task<Client> Delete(int id)
        {
            return await _repository.Remove(id);
        }

        public async Task<bool> EmailExist(string email)
        {
            return await _repository.EmailExist(email);
        }

        private ClientReponseDto MapToResponse(Client client)
        {
            return new ClientReponseDto
            {
                Id = client.Id,
                Nome = client.Nome,
                Email = client.Email,
                Ativo = client.Ativo,
                DataCadastro = client.DataCadastro
            };
        }
    }
}
