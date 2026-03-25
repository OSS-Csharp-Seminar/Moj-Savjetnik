using System.ComponentModel.DataAnnotations;

namespace MyAdvisor.Application.DTOs.Auth
{
    public record RefreshRequestDto([Required] string RefreshToken);
}
