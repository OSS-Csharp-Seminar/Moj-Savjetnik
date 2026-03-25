using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyAdvisor.Application.Interfaces.Contracts;
using MyAdvisor.Application.Interfaces.Repositories;
using MyAdvisor.Application.Interfaces.Services;
using MyAdvisor.Domain.Entities;
using MyAdvisor.Infrastructure.Auth;
using MyAdvisor.Infrastructure.Identity;

namespace MyAdvisor.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenGenerator _jwtGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _settings;

        public TokenService(
            JwtTokenGenerator jwtGenerator,
            IRefreshTokenRepository refreshTokenRepository,
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings)
        {
            _jwtGenerator = jwtGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
            _settings = jwtSettings.Value;
        }

        public string GenerateAccessToken(ITokenUser user)
            => _jwtGenerator.GenerateToken(user);

        public async Task<string> GenerateRefreshTokenAsync(ITokenUser user)
        {
            var token = new RefreshToken(
                Guid.NewGuid().ToString(),
                user.Id,
                DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays)
            );

            await _refreshTokenRepository.AddAsync(token);
            return token.Token;
        }

        public async Task<(string accessToken, string refreshToken)> RefreshAsync(string refreshToken)
        {
            var existingToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (existingToken == null || !existingToken.IsActive)
                throw new InvalidOperationException("Invalid or expired refresh token.");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.DomainUserId == existingToken.UserId);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            existingToken.Revoke();

            var newRefreshToken = new RefreshToken(
                Guid.NewGuid().ToString(),
                ((ITokenUser)user).Id,
                DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays)
            );

            await _refreshTokenRepository.UpdateAsync(existingToken);
            await _refreshTokenRepository.AddAsync(newRefreshToken);

            return (_jwtGenerator.GenerateToken(user), newRefreshToken.Token);
        }

        public async Task RevokeAsync(string refreshToken, int userId)
        {
            var existingToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (existingToken == null || !existingToken.IsActive)
                throw new InvalidOperationException("Invalid or already revoked token.");

            if (existingToken.UserId != userId)
                throw new UnauthorizedAccessException("Token does not belong to user.");

            existingToken.Revoke();
            await _refreshTokenRepository.UpdateAsync(existingToken);
        }
    }
}
