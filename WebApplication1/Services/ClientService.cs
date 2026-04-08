using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.DTOs;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;
        private readonly IPasswordService _passwordService;
        private readonly IUserRepository _userRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientService
            (IClientRepository repository,
            IPasswordService passwordService,
            IUserRepository userRepository,
            IProfileRepository profileRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _passwordService = passwordService;
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _httpContextAccessor = httpContextAccessor;
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
                DataCadastro = c.DataCadastro,
                Profiles = c.User.UserProfiles.Select(up => up.Profile.Nome).ToList(),
            });
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
                Password = await _passwordService.HashPassword(clientDto.Password),
                DataCadastro = DateTime.Now,

                UserProfiles = new List<UserProfile>
                {
                  new UserProfile { ProfileId = 3 } //Id do perfil "Cliente"
                }
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
            // 1. Busca o cliente com os Includes (Tracking habilitado)
            var existingClient = await _repository.GetById(clientUpdateDto.Id);

            if (existingClient == null)
            {
                return ServiceResult<ClientReponseDto>.Failure("Cliente não localizado");
            }

            // 2. Preparamos os IDs atuais para comparação
            var currentProfileIds = existingClient.User.UserProfiles.Select(up => up.ProfileId).ToList();

            // Verificamos se houve mudança nos perfis (comparando as listas de IDs)
            // Usamos SequenceEqual com OrderBy para garantir que a ordem não afete a comparação
            bool profilesChanged = !currentProfileIds.OrderBy(id => id)
                                    .SequenceEqual(clientUpdateDto.ProfileIds.OrderBy(id => id));

            // Verificamos se algo mudou no objeto geral
            if (existingClient.Nome == clientUpdateDto.Nome &&
                existingClient.Email == clientUpdateDto.Email &&
                existingClient.Ativo == clientUpdateDto.Ativo &&
                !profilesChanged)
            {
                return ServiceResult<ClientReponseDto>.NoChanges(MapToResponse(existingClient));
            }

            // 3. Se houve mudança de perfil, validamos se é Admin
            if (profilesChanged)
            {
                var usuarioLogadoIsAdmin = _httpContextAccessor.HttpContext.User.IsInRole("Admin");

                if (!usuarioLogadoIsAdmin)
                {
                    return ServiceResult<ClientReponseDto>.Failure("Apenas administradores podem alterar perfis de acesso.");
                }

                // A. REMOÇÃO: Percorremos o que está no BANCO.
                // Se o perfil do banco não está na lista que veio da tela, removemos.
                foreach (var existingProfile in existingClient.User.UserProfiles.ToList()) // ToList() evita erro de coleção modificada
                {
                    if (!clientUpdateDto.ProfileIds.Contains(existingProfile.ProfileId))
                    {
                        existingClient.User.UserProfiles.Remove(existingProfile);
                    }
                }

                // B. ADIÇÃO: Percorremos o que veio da TELA.
                // Se o ID da tela não está no banco, adicionamos um novo objeto.
                foreach (var profileId in clientUpdateDto.ProfileIds)
                {
                    if (!currentProfileIds.Contains(profileId))
                    {
                        existingClient.User.UserProfiles.Add(new UserProfile
                        {
                            ProfileId = profileId,
                            UserId = existingClient.UserId // O EF costuma preencher só com a coleção, mas é boa prática
                        });
                    }
                }
            }

            // 4. Atualiza propriedades básicas
            existingClient.Nome = clientUpdateDto.Nome;
            existingClient.Email = clientUpdateDto.Email;
            existingClient.Ativo = clientUpdateDto.Ativo;

            // 5. Salva as mudanças (O EF gerará os INSERTS e DELETES automaticamente)
            await _repository.SaveChangesAsync();
            return ServiceResult<ClientReponseDto>.Ok(MapToResponse(existingClient));

        }

        public async Task<ServiceResult<ClientReponseDto>> Delete(int id)
        {
            var client = await _repository.GetById(id);

            if (client == null)
            {
                ServiceResult<ClientReponseDto>.Failure("Usuário não localizado! ");
                return null;
            }

            var response = new ClientReponseDto
            {
                Id = client.Id,
                Nome = client.Nome,
                Email = client.Email,
                Ativo = client.Ativo,
                DataCadastro = client.DataCadastro
            };

            //await _repository.Remove(user.Id);
            await _repository.Remove(id);
            await _userRepository.Remove(client.UserId);
            await _repository.SaveChangesAsync();
            return ServiceResult<ClientReponseDto>.Ok(response);

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
                DataCadastro = client.DataCadastro,
                Profiles = client.User.UserProfiles.Select(up => up.Profile?.Nome ?? "Perfil não carregado")
                .ToList(),
            };
        }
    }
}
