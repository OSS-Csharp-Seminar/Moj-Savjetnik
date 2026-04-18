namespace MyAdvisor.Application.Interfaces.Services.AI
{
    public interface IGeminiService
    {
        Task<string> AnalyzeImageAsync(byte[] imageData, string mimeType, string prompt);
    }
}
