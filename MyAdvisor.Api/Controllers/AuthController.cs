using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyAdvisor.Application.DTOs.Auth;
using MyAdvisor.Application.DTOs.Common;
using MyAdvisor.Application.Interfaces.Services;

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
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
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
                return Unauthorized(new ErrorResponse("Invalid email or password."));
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
                return Unauthorized(new ErrorResponse(ex.Message));
            }
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(RevokeRequestDto request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized();
            try
            {
                await _authService.RevokeAsync(request, userId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }
    }
}
