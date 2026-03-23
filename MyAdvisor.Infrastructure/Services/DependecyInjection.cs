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
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();

            services.Configure<JwtSettings>(
            configuration.GetSection("JwtSettings"));

            services.AddSingleton<JwtTokenGenerator>();
            services.AddSingleton<TokenValidationParametersFactory>();

            return services;
        }
    }
}