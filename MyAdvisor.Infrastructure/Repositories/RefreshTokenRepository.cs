using Microsoft.EntityFrameworkCore;
using MyAdvisor.Application.Interfaces.Repositories;
using MyAdvisor.Domain.Entities;
using MyAdvisor.Infrastructure.Persistence;

namespace MyAdvisor.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _db;

        public RefreshTokenRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<RefreshToken?> GetByTokenAsync(string token)
            => _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);

        public async Task AddAsync(RefreshToken token)
        {
            await _db.RefreshTokens.AddAsync(token);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _db.RefreshTokens.Update(token);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteExpiredAndRevokedAsync()
        {
            await _db.Database.ExecuteSqlRawAsync(
                "DELETE FROM `RefreshTokens` WHERE `IsRevoked` = TRUE OR `ExpiryDate` < UTC_TIMESTAMP()");
        }
    }
}
