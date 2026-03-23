using Microsoft.EntityFrameworkCore;
using MyAdvisor.Application.Interfaces.Repositories;
using MyAdvisor.Domain.Entities;
using MyAdvisor.Infrastructure.Persistence;

namespace MyAdvisor.Infrastructure.Repositories
{
    public class SpendingStatisticRepository : ISpendingStatisticRepository
    {
        private readonly AppDbContext _db;

        public SpendingStatisticRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<SpendingStatistic?> GetByIdAsync(int id)
            => _db.SpendingStatistics.FirstOrDefaultAsync(s => s.Id == id);

        public async Task<IReadOnlyList<SpendingStatistic>> GetByUserIdAsync(int userId)
            => await _db.SpendingStatistics.Where(s => s.UserId == userId).ToListAsync();

        public Task<SpendingStatistic?> GetByUserIdAndPeriodAsync(int userId, int month, int year)
            => _db.SpendingStatistics.FirstOrDefaultAsync(s => s.UserId == userId && s.Month == month && s.Year == year);

        public async Task AddAsync(SpendingStatistic statistic)
        {
            await _db.SpendingStatistics.AddAsync(statistic);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(SpendingStatistic statistic)
        {
            _db.SpendingStatistics.Update(statistic);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var statistic = await _db.SpendingStatistics.FindAsync(id);
            if (statistic is not null)
            {
                _db.SpendingStatistics.Remove(statistic);
                await _db.SaveChangesAsync();
            }
        }
    }
}
