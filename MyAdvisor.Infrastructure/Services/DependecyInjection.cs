using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAdvisor.Application.Interfaces;
using MyAdvisor.Infrastructure.Auth;
using MyAdvisor.Infrastructure.Identity;
using MyAdvisor.Infrastructure.Persistence;

namespace MyAdvisor.Infrastructure.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
                ?? throw new InvalidOperationException("JwtSettings are not configured.");

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySQL(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParametersFactory(
                    Microsoft.Extensions.Options.Options.Create(jwtSettings)).Create();
            });

            services.AddSingleton<JwtTokenGenerator>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
