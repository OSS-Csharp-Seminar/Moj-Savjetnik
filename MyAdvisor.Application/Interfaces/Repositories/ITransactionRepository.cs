using MyAdvisor.Domain.Entities;

namespace MyAdvisor.Application.Interfaces.Repositories
{
    // Read-only. All writes go through FinancialDiary aggregate root.
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(int id);
        Task<IReadOnlyList<Transaction>> GetByDiaryIdAsync(int diaryId);
    }
}
