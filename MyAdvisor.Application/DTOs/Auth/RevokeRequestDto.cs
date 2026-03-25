using System.ComponentModel.DataAnnotations;

namespace MyAdvisor.Application.DTOs.Auth
{
    public record RevokeRequestDto([Required] string RefreshToken);
}
