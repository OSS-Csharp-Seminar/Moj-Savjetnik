using MyAdvisor.Domain.Entities;

namespace MyAdvisor.Application.Interfaces.Repositories
{
    public interface IRecurringTransactionRepository
    {
        Task<RecurringTransaction?> GetByIdAsync(int id);
        Task<IReadOnlyList<RecurringTransaction>> GetByUserIdAsync(int userId);
        Task AddAsync(RecurringTransaction recurringTransaction);
        Task UpdateAsync(RecurringTransaction recurringTransaction);
        Task DeleteAsync(int id);
    }
}
