using System.ComponentModel.DataAnnotations;

namespace MyAdvisor.Application.DTOs.Auth
{
    public record RegisterRequestDto(
        [Required][EmailAddress] string Email,
        [Required][MinLength(8)] string Password,
        [Required][MaxLength(100)] string FirstName,
        [Required][MaxLength(100)] string LastName
    );
}
