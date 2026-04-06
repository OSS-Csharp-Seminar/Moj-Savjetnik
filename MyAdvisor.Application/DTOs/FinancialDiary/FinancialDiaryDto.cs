using MyAdvisor.Application.DTOs.Transaction;

namespace MyAdvisor.Application.DTOs.FinancialDiary
{
    public record FinancialDiaryDto(
        int Id,
        int UserId,
        DateTime Date,
        decimal TotalAmount,
        string? Notes,
        IReadOnlyList<TransactionDto> Transactions
    );
}
