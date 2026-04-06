using System.ComponentModel.DataAnnotations;
using MyAdvisor.Domain.Enums;

namespace MyAdvisor.Application.DTOs.Transaction
{
    public record AddTransactionRequestDto(
        [Required] int DiaryId,
        [Required] decimal Amount,
        int? CategoryId,
        string? Description,
        DateTime? TransactionDate,
        [Required] PaymentMethod? PaymentMethod
    );
}
