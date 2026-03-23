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
            try
            {
                var result = await _authService.RegisterAsync(request);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequestDto request)
        {
            try
            {
                var result = await _authService.RefreshAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(RevokeRequestDto request)
        {
            try
            {
                await _authService.RevokeAsync(request);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
