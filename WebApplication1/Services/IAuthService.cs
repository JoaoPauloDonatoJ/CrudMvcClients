using WebApplication1.Models;
using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public interface IAuthService
    {
        Task<ServiceResult<UserResponseDto>> Authenticate(LoginDto loginDto);
    }
}
