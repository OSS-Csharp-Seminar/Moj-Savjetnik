using Microsoft.AspNetCore.Identity;
using MyAdvisor.Application.Interfaces;

namespace MyAdvisor.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<int>, ITokenUser
    {
        public int DomainUserId { get; set; }

        // ITokenUser uses the domain User's ID, not the Identity ID
        int ITokenUser.Id => DomainUserId;
        string ITokenUser.Email => Email ?? string.Empty;
    }
}
