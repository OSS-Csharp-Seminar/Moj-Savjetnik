using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace MyAdvisor.Infrastructure.Auth
{
    public class TokenValidationParametersFactory
    {
        private readonly JwtSettings _settings;
        public TokenValidationParametersFactory(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }
        public TokenValidationParameters Create()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_settings.SecretKey)
                ),

                ClockSkew = TimeSpan.Zero
            };
        }
    }
}