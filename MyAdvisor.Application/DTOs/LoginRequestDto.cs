using System.ComponentModel.DataAnnotations;

namespace MyAdvisor.Application.DTOs
{
    public record LoginRequestDto(
        [Required][EmailAddress] string Email,
        [Required] string Password
    );
}
