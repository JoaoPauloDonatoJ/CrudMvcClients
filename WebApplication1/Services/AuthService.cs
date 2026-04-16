using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class AuthService : IAuthService
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserRepository _userRepository;
        

        public AuthService(IPasswordService passwordService, IClientRepository clientRepository, IUserRepository userRepository)
        {
            _passwordService = passwordService;
            _userRepository = userRepository;
        }

        public async Task<ServiceResult<UserResponseDto>> Authenticate(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmail(loginDto.Email);

            if (user == null)
            {
                return ServiceResult<UserResponseDto>.Failure("Usuário não localizado !");
            }

            var isPasswordValid = await _passwordService.VerifyPassword(loginDto.Password, user.Password);

            if (!isPasswordValid)
            {
                return ServiceResult<UserResponseDto>.Failure("Usuário ou senha inválidos");
            }

            var response = new UserResponseDto
            {
                Id = user.Id,
                Nome = user.Nome,
                Email = user.Email,
                Ativo = user.Ativo,
                DataCadastro = user.DataCadastro,
                Excluido = user.Excluido,
                DataExclusao = user.DataExclusao,
                Profiles = user.UserProfiles.Select(up => up.Profile.Nome).ToList() // Mapeia os nomes aqui
            };

            return ServiceResult<UserResponseDto>.Ok(response);
        }
    }
}
