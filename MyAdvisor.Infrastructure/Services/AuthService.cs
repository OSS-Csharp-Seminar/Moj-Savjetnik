using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyAdvisor.Application.DTOs;
using MyAdvisor.Application.Interfaces;
using MyAdvisor.Domain.Entities;
using MyAdvisor.Infrastructure.Identity;
using MyAdvisor.Infrastructure.Persistence;

namespace MyAdvisor.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _dbContext;

        public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService, AppDbContext dbContext)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _dbContext = dbContext;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var domainUser = new User($"{request.FirstName} {request.LastName}", request.Email);
            await _dbContext.DomainUsers.AddAsync(domainUser);
            await _dbContext.SaveChangesAsync();

            var identityUser = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                DomainUserId = domainUser.Id
            };

            var result = await _userManager.CreateAsync(identityUser, request.Password);

            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            domainUser.SetPasswordHash(identityUser.PasswordHash!);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return new RegisterResponseDto("User created");
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new UnauthorizedAccessException();

            if (!await _userManager.CheckPasswordAsync(identityUser, request.Password))
                throw new UnauthorizedAccessException();

            var accessToken = _tokenService.GenerateAccessToken(identityUser);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(identityUser);

            return new AuthResponseDto(accessToken, refreshToken);
        }

        public async Task<AuthResponseDto> RefreshAsync(RefreshRequestDto request)
        {
            var result = await _tokenService.RefreshAsync(request.RefreshToken);
            return new AuthResponseDto(result.accessToken, result.refreshToken);
        }

        public Task RevokeAsync(RevokeRequestDto request)
            => _tokenService.RevokeAsync(request.RefreshToken);
    }
}
