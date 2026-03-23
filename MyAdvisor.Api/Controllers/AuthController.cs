using Microsoft.AspNetCore.Mvc;
using MyAdvisor.Application.DTOs;
using MyAdvisor.Application.Interfaces;

namespace MyAdvisor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequestDto request)
        {
            var result = await _authService.RefreshAsync(request);
            return Ok(result);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(RevokeRequestDto request)
        {
            await _authService.RevokeAsync(request);
            return Ok();
        }
    }
}
