using MyAdvisor.Domain.Entities;

namespace MyAdvisor.Application.Interfaces.Repositories
{
    public interface ITransactionAiLogRepository
    {
        Task<TransactionAiLog?> GetByIdAsync(int id);
        Task<IReadOnlyList<TransactionAiLog>> GetByTransactionIdAsync(int transactionId);
        Task AddAsync(TransactionAiLog log);
    }
}
