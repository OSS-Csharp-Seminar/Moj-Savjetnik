using MyAdvisor.Application.DTOs.AI;

namespace MyAdvisor.Application.Interfaces.Services.App
{
    public interface IAiTransactionImportService
    {
        Task<AiTransactionImportResultDto> ImportFromImageAsync(
            int diaryId,
            int userId,
            byte[] imageData,
            string mimeType);
    }
}
