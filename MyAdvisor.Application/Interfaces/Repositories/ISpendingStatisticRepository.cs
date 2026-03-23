using MyAdvisor.Domain.Entities;

namespace MyAdvisor.Application.Interfaces.Repositories
{
    public interface ISpendingStatisticRepository
    {
        Task<SpendingStatistic?> GetByIdAsync(int id);
        Task<IReadOnlyList<SpendingStatistic>> GetByUserIdAsync(int userId);
        Task<SpendingStatistic?> GetByUserIdAndPeriodAsync(int userId, int month, int year);
        Task AddAsync(SpendingStatistic statistic);
        Task UpdateAsync(SpendingStatistic statistic);
        Task DeleteAsync(int id);
    }
}
