using MyAdvisor.Application.DTOs.FinancialDiary;

namespace MyAdvisor.Application.Interfaces.Services.Domain
{
    public interface IFinancialDiaryService
    {
        Task<FinancialDiaryDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<FinancialDiarySummaryDto>> GetAllAsync(int userId);
        Task<FinancialDiaryDto> CreateAsync(CreateFinancialDiaryRequestDto request, int userId);
        Task<FinancialDiaryDto> UpdateAsync(int id, UpdateFinancialDiaryRequestDto request);
        Task UpdateTotalAsync(int id, decimal total);
        Task DeleteAsync(int id);
    }
}
