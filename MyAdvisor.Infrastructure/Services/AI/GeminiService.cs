using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MyAdvisor.Application.Interfaces.Services.AI;
using MyAdvisor.Infrastructure.AI;

namespace MyAdvisor.Infrastructure.Services.AI
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _http;
        private readonly GeminiSettings _settings;

        public GeminiService(HttpClient http, IOptions<GeminiSettings> settings)
        {
            _http = http;
            _settings = settings.Value;
        }

        public async Task<string> AnalyzeImageAsync(byte[] imageData, string mimeType, string prompt)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new
                            {
                                inline_data = new
                                {
                                    mime_type = mimeType,
                                    data = Convert.ToBase64String(imageData)
                                }
                            },
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.1,
                    maxOutputTokens = 2048
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(url, content);
            var rawJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Gemini API error ({(int)response.StatusCode}): {rawJson}");

            using var doc = JsonDocument.Parse(rawJson);

            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? string.Empty;
        }
    }
}
