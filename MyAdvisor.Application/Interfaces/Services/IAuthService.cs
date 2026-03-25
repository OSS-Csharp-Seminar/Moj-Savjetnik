using MyAdvisor.Application.DTOs.Auth;

namespace MyAdvisor.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> RefreshAsync(RefreshRequestDto request);
        Task RevokeAsync(RevokeRequestDto request, int userId);
    }
}
