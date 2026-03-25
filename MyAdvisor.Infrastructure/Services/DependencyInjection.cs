using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAdvisor.Application.Interfaces;
using MyAdvisor.Application.Interfaces.Repositories;
using MyAdvisor.Application.Interfaces.Services;
using MyAdvisor.Application.Mappers;
using MyAdvisor.Infrastructure.Auth;
using MyAdvisor.Infrastructure.Identity;
using MyAdvisor.Infrastructure.Persistence;
using MyAdvisor.Infrastructure.Repositories;
using UnitOfWork = MyAdvisor.Infrastructure.Persistence.UnitOfWork;

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

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<JwtTokenGenerator>();
            services.AddScoped<UserMapper>();
            services.AddScoped<IdentityService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IFinancialDiaryRepository, FinancialDiaryRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
            services.AddScoped<ITransactionAiLogRepository, TransactionAiLogRepository>();
            services.AddScoped<ISpendingStatisticRepository, SpendingStatisticRepository>();

            services.AddHostedService<RefreshTokenCleanupService>();

            return services;
        }
    }
}
