using MyAdvisor.Domain.Enums;

namespace MyAdvisor.Application.DTOs.Transaction
{
    public record TransactionDto(
        int Id,
        int DiaryId,
        decimal Amount,
        int? CategoryId,
        string? Description,
        DateTime TransactionDate,
        PaymentMethod? PaymentMethod
    );
}
