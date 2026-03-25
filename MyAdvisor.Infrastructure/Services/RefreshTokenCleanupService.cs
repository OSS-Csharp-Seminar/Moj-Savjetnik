using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyAdvisor.Application.Interfaces.Repositories;

namespace MyAdvisor.Infrastructure.Services
{
    public class RefreshTokenCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RefreshTokenCleanupService> _logger;
        private static readonly TimeSpan Interval = TimeSpan.FromDays(1);

        public RefreshTokenCleanupService(IServiceScopeFactory scopeFactory, ILogger<RefreshTokenCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanupAsync();
                await Task.Delay(Interval, stoppingToken);
            }
        }

        private async Task CleanupAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();

            await repository.DeleteExpiredAndRevokedAsync();
            _logger.LogInformation("Expired and revoked refresh tokens cleaned up.");
        }
    }
}
