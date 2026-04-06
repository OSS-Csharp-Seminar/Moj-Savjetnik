using System.ComponentModel.DataAnnotations;
using MyAdvisor.Domain.Enums;

namespace MyAdvisor.Application.DTOs.Transaction
{
    public record UpdateTransactionRequestDto(
        [Required] decimal Amount,
        int? CategoryId,
        string? Description,
        DateTime? TransactionDate,
        [Required] PaymentMethod? PaymentMethod
    );
}
