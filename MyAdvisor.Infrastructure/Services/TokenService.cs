using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyAdvisor.Application.Interfaces;
using MyAdvisor.Domain.Entities;
using MyAdvisor.Infrastructure.Auth;
using MyAdvisor.Infrastructure.Identity;
using MyAdvisor.Infrastructure.Persistence;

namespace MyAdvisor.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenGenerator _jwtGenerator;
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(JwtTokenGenerator jwtGenerator, AppDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _jwtGenerator = jwtGenerator;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public string GenerateAccessToken(ITokenUser user)
            => _jwtGenerator.GenerateToken(user);

        public async Task<string> GenerateRefreshTokenAsync(ITokenUser user)
        {
            var token = new RefreshToken(
                Guid.NewGuid().ToString(),
                user.Id,
                DateTime.UtcNow.AddDays(7)
            );

            await _dbContext.RefreshTokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();

            return token.Token;
        }

        public async Task<(string accessToken, string refreshToken)> RefreshAsync(string refreshToken)
        {
            var existingToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken);

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
                DateTime.UtcNow.AddDays(7)
            );

            await _dbContext.RefreshTokens.AddAsync(newRefreshToken);
            await _dbContext.SaveChangesAsync();

            return (_jwtGenerator.GenerateToken(user), newRefreshToken.Token);
        }

        public async Task RevokeAsync(string refreshToken)
        {
            var existingToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken);

            if (existingToken == null || !existingToken.IsActive)
                throw new InvalidOperationException("Invalid or already revoked token.");

            existingToken.Revoke();
            await _dbContext.SaveChangesAsync();
        }
    }
}
