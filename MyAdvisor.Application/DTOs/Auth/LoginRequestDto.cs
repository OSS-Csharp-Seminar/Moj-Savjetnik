using System.ComponentModel.DataAnnotations;

namespace MyAdvisor.Application.DTOs.Auth
{
    public record LoginRequestDto(
        [Required][EmailAddress] string Email,
        [Required] string Password
    );
}
