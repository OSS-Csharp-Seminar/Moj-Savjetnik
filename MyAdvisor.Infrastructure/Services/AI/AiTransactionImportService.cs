using System.Text.Json;
using MyAdvisor.Application.DTOs.AI;
using MyAdvisor.Application.DTOs.Transaction;
using MyAdvisor.Application.Interfaces.Repositories;
using MyAdvisor.Application.Interfaces.Services.AI;
using MyAdvisor.Application.Interfaces.Services.App;
using MyAdvisor.Application.Interfaces.Services.Domain;
using MyAdvisor.Domain.Entities;
using MyAdvisor.Domain.Enums;

namespace MyAdvisor.Infrastructure.Services.AI
{
    public class AiTransactionImportService : IAiTransactionImportService
    {
        private readonly IGeminiService _gemini;
        private readonly IFinancialDiaryService _diaryService;
        private readonly ITransactionService _transactionService;
        private readonly ITransactionAiLogRepository _aiLogRepository;
        private readonly ICategoryRepository _categoryRepository;

        public AiTransactionImportService(
            IGeminiService gemini,
            IFinancialDiaryService diaryService,
            ITransactionService transactionService,
            ITransactionAiLogRepository aiLogRepository,
            ICategoryRepository categoryRepository)
        {
            _gemini = gemini;
            _diaryService = diaryService;
            _transactionService = transactionService;
            _aiLogRepository = aiLogRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<AiTransactionImportResultDto> ImportFromImageAsync(
            int diaryId,
            int userId,
            byte[] imageData,
            string mimeType)
        {
            var diary = await _diaryService.GetByIdAsync(diaryId)
                ?? throw new KeyNotFoundException($"Diary {diaryId} not found.");

            if (diary.UserId != userId)
                throw new UnauthorizedAccessException();

            var categories = await _categoryRepository.GetAllAsync();
            var categoryNames = string.Join(", ", categories.Select(c => c.Name));

            var rawResponse = await _gemini.AnalyzeImageAsync(imageData, mimeType, BuildPrompt(categoryNames));
            var parsedItems = ParseGeminiResponse(rawResponse);

            var imported = new List<TransactionDto>();

            foreach (var item in parsedItems)
            {
                try
                {
                    var matchedCategory = categories.FirstOrDefault(c =>
                        string.Equals(c.Name, item.CategoryName, StringComparison.OrdinalIgnoreCase));

                    DateTime? transactionDate = DateTime.TryParse(item.TransactionDate, out var parsedDate)
                        ? parsedDate
                        : null;

                    var request = new AddTransactionRequestDto(
                        DiaryId: diaryId,
                        Amount: item.Amount,
                        CategoryId: matchedCategory?.Id,
                        Description: item.Description,
                        TransactionDate: transactionDate,
                        PaymentMethod: ParsePaymentMethod(item.PaymentMethod)
                    );

                    var transactionDto = await _transactionService.AddAsync(request);
                    imported.Add(transactionDto);

                    var snippet = rawResponse.Length > 2000 ? rawResponse[..2000] : rawResponse;
                    var confidence = matchedCategory is not null
                        ? Math.Clamp(item.Confidence, 0.5m, 1.0m)
                        : Math.Clamp(item.Confidence * 0.6m, 0m, 1m);

                    var log = new TransactionAiLog(
                        transactionId: transactionDto.Id,
                        rawOcrText: snippet,
                        aiCategoryId: matchedCategory?.Id,
                        aiConfidence: confidence
                    );

                    await _aiLogRepository.AddAsync(log);
                }
                catch
                {
                    // skip malformed items
                }
            }

            var newTotal = await _transactionService.GetTotalByDiaryIdAsync(diaryId);
            await _diaryService.UpdateTotalAsync(diaryId, newTotal);

            return new AiTransactionImportResultDto(
                ImportedTransactions: imported,
                TotalFound: parsedItems.Count,
                SuccessfullyImported: imported.Count
            );
        }

        private static string BuildPrompt(string categoryNames) => $$"""
            You are a precise financial transaction extractor.
            Analyze the image (bank statement, receipt, expense list, or similar document) and extract EVERY transaction visible.

            Respond ONLY with a raw JSON array. No explanation, no markdown, no code fences — just the array itself.

            Each object must have EXACTLY these fields:
            - "amount":          number  — negative for expenses/payments, positive for income/deposits
            - "description":     string or null
            - "categoryName":    string — pick the best match from: {{categoryNames}}
            - "paymentMethod":   string — one of: Cash, Card, Transfer, Other
            - "transactionDate": string in "yyyy-MM-dd" format (use today's date if not visible)
            - "confidence":      number between 0.0 and 1.0

            Example:
            [{"amount":-12.50,"description":"Coffee Shop","categoryName":"Food","paymentMethod":"Card","transactionDate":"2025-03-01","confidence":0.95}]

            If no transactions are found, return: []
            """;

        private static List<ParsedTransaction> ParseGeminiResponse(string raw)
        {
            try
            {
                var cleaned = raw.Trim();

                if (cleaned.StartsWith("```"))
                {
                    var firstNewline = cleaned.IndexOf('\n');
                    if (firstNewline >= 0)
                        cleaned = cleaned[(firstNewline + 1)..];

                    var lastFence = cleaned.LastIndexOf("```");
                    if (lastFence >= 0)
                        cleaned = cleaned[..lastFence];

                    cleaned = cleaned.Trim();
                }

                return JsonSerializer.Deserialize<List<ParsedTransaction>>(
                    cleaned,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? [];
            }
            catch
            {
                return [];
            }
        }

        private static PaymentMethod? ParsePaymentMethod(string? raw) =>
            raw?.Trim().ToLowerInvariant() switch
            {
                "cash"     => PaymentMethod.Cash,
                "card"     => PaymentMethod.Card,
                "transfer" => PaymentMethod.Transfer,
                _          => PaymentMethod.Other
            };

        private sealed class ParsedTransaction
        {
            public decimal Amount { get; set; }
            public string? Description { get; set; }
            public string? CategoryName { get; set; }
            public string? PaymentMethod { get; set; }
            public string? TransactionDate { get; set; }
            public decimal Confidence { get; set; }
        }
    }
}
