using MyAdvisor.Application.DTOs;

namespace MyAdvisor.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> RefreshAsync(RefreshRequestDto request);
        Task RevokeAsync(RevokeRequestDto request);
    }
}
