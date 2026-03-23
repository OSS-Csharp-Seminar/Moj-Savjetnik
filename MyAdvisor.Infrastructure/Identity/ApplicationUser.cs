using Microsoft.AspNetCore.Identity;
using MyAdvisor.Application.Interfaces;

namespace MyAdvisor.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<int>, ITokenUser
    {
        public int DomainUserId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
