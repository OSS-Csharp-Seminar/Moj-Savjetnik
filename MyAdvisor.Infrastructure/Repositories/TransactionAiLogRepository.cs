using Microsoft.EntityFrameworkCore;
using MyAdvisor.Application.Interfaces.Repositories;
using MyAdvisor.Domain.Entities;
using MyAdvisor.Infrastructure.Persistence;

namespace MyAdvisor.Infrastructure.Repositories
{
    public class TransactionAiLogRepository : ITransactionAiLogRepository
    {
        private readonly AppDbContext _db;

        public TransactionAiLogRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<TransactionAiLog?> GetByIdAsync(int id)
            => _db.TransactionAiLogs.FirstOrDefaultAsync(l => l.Id == id);

        public async Task<IReadOnlyList<TransactionAiLog>> GetByTransactionIdAsync(int transactionId)
            => await _db.TransactionAiLogs.Where(l => l.TransactionId == transactionId).ToListAsync();

        public async Task AddAsync(TransactionAiLog log)
        {
            await _db.TransactionAiLogs.AddAsync(log);
            await _db.SaveChangesAsync();
        }
    }
}
