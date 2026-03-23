using MyAdvisor.Application.Interfaces;
using MyAdvisor.Domain.Entities;
using MyAdvisor.Infrastructure.Auth;

namespace MyAdvisor.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenGenerator _jwtGenerator;

        public TokenService(JwtTokenGenerator jwtGenerator)
        {
            _jwtGenerator = jwtGenerator;
        }
        public string GenerateAccessToken(ITokenUser user)
        {
            return _jwtGenerator.GenerateToken(user);
        }
        public async Task<RefreshToken> GenerateRefreshTokenAsync(ITokenUser user)
        {
            var token = Guid.NewGuid().ToString();

            return new RefreshToken(
                token,
                user.Id,
                DateTime.UtcNow.AddDays(7)
            );
        }
        public async Task<(string accessToken, RefreshToken refreshToken)> RefreshAsync(string refreshToken)
        {
            // validate from DB (not shown here)

            var newAccessToken = "new_access_token";

            var newRefreshToken = new RefreshToken(
                Guid.NewGuid().ToString(),
                1,
                DateTime.UtcNow.AddDays(7)
            );

            return (newAccessToken, newRefreshToken);
        }

        public async Task RevokeAsync(string refreshToken)
        {
            // revoke logic (DB update)
        }
    }
}