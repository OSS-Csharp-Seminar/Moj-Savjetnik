using MyAdvisor.Application.Interfaces.Contracts;

namespace MyAdvisor.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(ITokenUser user);
        Task<string> GenerateRefreshTokenAsync(ITokenUser user);
        Task<(string accessToken, string refreshToken)> RefreshAsync(string refreshToken);
        Task RevokeAsync(string refreshToken, int userId);
    }
}
