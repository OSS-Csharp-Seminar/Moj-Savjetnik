using MyAdvisor.Application.DTOs.Transaction;

namespace MyAdvisor.Application.Interfaces.Services.Domain
{
    public interface ITransactionService
    {
        Task<TransactionDto> GetByIdAsync(int id);
        Task<IReadOnlyList<TransactionDto>> GetByDiaryIdAsync(int diaryId);
        Task<decimal> GetTotalByDiaryIdAsync(int diaryId);
        Task<TransactionDto> AddAsync(AddTransactionRequestDto request);
        Task<TransactionDto> UpdateAsync(int id, UpdateTransactionRequestDto request);
        Task DeleteAsync(int id);
    }
}
