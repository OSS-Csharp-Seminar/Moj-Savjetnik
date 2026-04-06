using System.ComponentModel.DataAnnotations;

namespace MyAdvisor.Application.DTOs.FinancialDiary
{
    public record CreateFinancialDiaryRequestDto(
        [Required] DateTime Date,
        string? Notes
    );
}
