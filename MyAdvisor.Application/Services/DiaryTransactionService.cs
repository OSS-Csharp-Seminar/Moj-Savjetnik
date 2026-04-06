using MyAdvisor.Application.DTOs.Transaction;
using MyAdvisor.Application.Interfaces.Services.App;
using MyAdvisor.Application.Interfaces.Services.Domain;

namespace MyAdvisor.Application.Services
{
    public class DiaryTransactionService : IDiaryTransactionService
    {
        private readonly IFinancialDiaryService _diaryService;
        private readonly ITransactionService _transactionService;

        public DiaryTransactionService(
            IFinancialDiaryService diaryService,
            ITransactionService transactionService)
        {
            _diaryService = diaryService;
            _transactionService = transactionService;
        }

        public Task<TransactionDto> GetByIdAsync(int id)
            => _transactionService.GetByIdAsync(id);

        public Task<IReadOnlyList<TransactionDto>> GetByDiaryIdAsync(int diaryId)
            => _transactionService.GetByDiaryIdAsync(diaryId);

        public async Task<TransactionDto> AddAsync(AddTransactionRequestDto request, int userId)
        {
            var diary = await _diaryService.GetByIdAsync(request.DiaryId)
                ?? throw new KeyNotFoundException($"Diary {request.DiaryId} not found.");

            if (diary.UserId != userId)
                throw new UnauthorizedAccessException();

            var transaction = await _transactionService.AddAsync(request);

            var newTotal = await _transactionService.GetTotalByDiaryIdAsync(request.DiaryId);
            await _diaryService.UpdateTotalAsync(request.DiaryId, newTotal);

            return transaction;
        }

        public async Task<TransactionDto> UpdateAsync(int transactionId, UpdateTransactionRequestDto request, int userId)
        {
            var transaction = await _transactionService.GetByIdAsync(transactionId);

            var diary = await _diaryService.GetByIdAsync(transaction.DiaryId)
                ?? throw new KeyNotFoundException($"Diary {transaction.DiaryId} not found.");

            if (diary.UserId != userId)
                throw new UnauthorizedAccessException();

            var updated = await _transactionService.UpdateAsync(transactionId, request);

            var newTotal = await _transactionService.GetTotalByDiaryIdAsync(transaction.DiaryId);
            await _diaryService.UpdateTotalAsync(transaction.DiaryId, newTotal);

            return updated;
        }

        public async Task DeleteAsync(int transactionId, int userId)
        {
            var transaction = await _transactionService.GetByIdAsync(transactionId);

            var diary = await _diaryService.GetByIdAsync(transaction.DiaryId)
                ?? throw new KeyNotFoundException($"Diary {transaction.DiaryId} not found.");

            if (diary.UserId != userId)
                throw new UnauthorizedAccessException();

            await _transactionService.DeleteAsync(transactionId);

            var newTotal = await _transactionService.GetTotalByDiaryIdAsync(transaction.DiaryId);
            await _diaryService.UpdateTotalAsync(transaction.DiaryId, newTotal);
        }
    }
}
