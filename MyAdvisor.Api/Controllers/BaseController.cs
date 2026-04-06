using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MyAdvisor.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected int? ResolveUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            return int.TryParse(claim, out var id) ? id : null;
        }
    }
}
