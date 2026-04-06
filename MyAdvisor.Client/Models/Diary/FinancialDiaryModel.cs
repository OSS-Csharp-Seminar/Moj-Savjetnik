namespace MyAdvisor.Client.Models.Diary;

public record FinancialDiaryModel(
    int Id,
    int UserId,
    DateTime Date,
    decimal TotalAmount,
    string? Notes
);
