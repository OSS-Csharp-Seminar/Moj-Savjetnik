using Microsoft.AspNetCore.Identity;
using MyAdvisor.Application.DTOs;
using MyAdvisor.Application.Interfaces;
using MyAdvisor.Infrastructure.Identity;
using MyAdvisor.Domain.Entities;

namespace MyAdvisor.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return new RegisterResponseDto("User created");
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new UnauthorizedAccessException();

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
                throw new UnauthorizedAccessException();

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);

            return new AuthResponseDto(accessToken, refreshToken.Token);
        }

        public async Task<AuthResponseDto> RefreshAsync(RefreshRequestDto request)
        {
            var result = await _tokenService.RefreshAsync(request.RefreshToken);

            var accessToken = result.accessToken;
            var refreshToken = result.refreshToken;

            return new AuthResponseDto(accessToken, refreshToken.Token);
        }

        public Task RevokeAsync(RevokeRequestDto request)
            => _tokenService.RevokeAsync(request.RefreshToken);
    }
}
