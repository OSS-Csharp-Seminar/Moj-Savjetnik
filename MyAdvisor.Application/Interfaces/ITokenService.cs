namespace MyAdvisor.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(ITokenUser user);
        Task<string> GenerateRefreshTokenAsync(ITokenUser user);
        Task<(string accessToken, string refreshToken)> RefreshAsync(string refreshToken);
        Task RevokeAsync(string refreshToken);
    }
}
