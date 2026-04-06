using MyAdvisor.Application.DTOs.Transaction;

namespace MyAdvisor.Application.Interfaces.Services.App
{
    public interface IDiaryTransactionService
    {
        Task<TransactionDto> GetByIdAsync(int id);
        Task<IReadOnlyList<TransactionDto>> GetByDiaryIdAsync(int diaryId);
        Task<TransactionDto> AddAsync(AddTransactionRequestDto request, int userId);
        Task<TransactionDto> UpdateAsync(int transactionId, UpdateTransactionRequestDto request, int userId);
        Task DeleteAsync(int transactionId, int userId);
    }
}
