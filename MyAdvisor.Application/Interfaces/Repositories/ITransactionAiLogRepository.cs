using MyAdvisor.Domain.Entities;

namespace MyAdvisor.Application.Interfaces.Repositories
{
    // AI logs are append-only. Update and Delete are intentionally omitted.
    public interface ITransactionAiLogRepository
    {
        Task<TransactionAiLog?> GetByIdAsync(int id);
        Task<IReadOnlyList<TransactionAiLog>> GetByTransactionIdAsync(int transactionId);
        Task AddAsync(TransactionAiLog log);
    }
}
