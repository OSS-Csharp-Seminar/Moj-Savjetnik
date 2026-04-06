namespace MyAdvisor.Application.DTOs.FinancialDiary
{
    public record FinancialDiarySummaryDto(
        int Id,
        int UserId,
        DateTime Date,
        decimal TotalAmount,
        string? Notes
    );
}
