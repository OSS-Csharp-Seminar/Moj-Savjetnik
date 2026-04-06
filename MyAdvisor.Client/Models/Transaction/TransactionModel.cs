namespace MyAdvisor.Client.Models.Transaction;

public record TransactionModel(
    int Id,
    int DiaryId,
    decimal Amount,
    int? CategoryId,
    string? Description,
    DateTime TransactionDate,
    string? PaymentMethod
);
