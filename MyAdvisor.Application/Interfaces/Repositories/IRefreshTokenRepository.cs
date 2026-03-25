using MyAdvisor.Domain.Entities;

namespace MyAdvisor.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
        Task DeleteExpiredAndRevokedAsync();
    }
}
