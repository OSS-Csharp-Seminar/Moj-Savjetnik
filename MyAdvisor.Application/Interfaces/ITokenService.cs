using MyAdvisor.Domain.Entities;

namespace MyAdvisor.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(ITokenUser user);
        Task<RefreshToken> GenerateRefreshTokenAsync(ITokenUser user);
        Task<(string accessToken, RefreshToken refreshToken)> RefreshAsync(string refreshToken);
        Task RevokeAsync(string refreshToken);
    }
}
