using Microsoft.EntityFrameworkCore;
using MyAdvisor.Application.Interfaces.Repositories;
using MyAdvisor.Domain.Entities;
using MyAdvisor.Infrastructure.Persistence;

namespace MyAdvisor.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _db;

        public TransactionRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<Transaction?> GetByIdAsync(int id)
            => _db.Transactions.FirstOrDefaultAsync(t => t.Id == id);

        public async Task<IReadOnlyList<Transaction>> GetByDiaryIdAsync(int diaryId)
            => await _db.Transactions.Where(t => t.DiaryId == diaryId).ToListAsync();

        public Task<decimal> GetTotalByDiaryIdAsync(int diaryId)
            => _db.Transactions.Where(t => t.DiaryId == diaryId).SumAsync(t => t.Amount);

        public async Task AddAsync(Transaction transaction)
        {
            await _db.Transactions.AddAsync(transaction);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _db.Transactions.Update(transaction);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await _db.Transactions.FindAsync(id);
            if (transaction is not null)
            {
                _db.Transactions.Remove(transaction);
                await _db.SaveChangesAsync();
            }
        }
    }
}
