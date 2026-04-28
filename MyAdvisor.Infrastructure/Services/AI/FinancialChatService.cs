using MyAdvisor.Application.DTOs.AI;
using MyAdvisor.Application.Interfaces.Repositories;
using MyAdvisor.Application.Interfaces.Services.AI;
using MyAdvisor.Application.Interfaces.Services.App;

namespace MyAdvisor.Infrastructure.Services.AI
{
    public class FinancialChatService : IFinancialChatService
    {
        private readonly IGeminiService _gemini;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IFinancialDiaryRepository _diaryRepository;
        private readonly ICategoryRepository _categoryRepository;

        public FinancialChatService(
            IGeminiService gemini,
            ITransactionRepository transactionRepository,
            IFinancialDiaryRepository diaryRepository,
            ICategoryRepository categoryRepository)
        {
            _gemini = gemini;
            _transactionRepository = transactionRepository;
            _diaryRepository = diaryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ChatResponseDto> ChatAsync(int userId, ChatRequestDto request)
        {
            var systemPrompt = await BuildSystemPromptAsync(userId, request.IncludeFinancialContext);
            var history = request.History.Select(m => (m.Role, m.Content)).ToList();
            var reply = await _gemini.ChatAsync(systemPrompt, history, request.Message);
            return new ChatResponseDto(reply);
        }

        private async Task<string> BuildSystemPromptAsync(int userId, bool includeContext)
        {
            var basePrompt = """
                You are a knowledgeable and friendly personal financial advisor built into MyAdvisor.
                You help users understand their spending habits, give budgeting advice, explain financial concepts,
                and answer any questions about banking, saving, investing, or money management.
                Be concise, practical, and supportive. Use clear language and avoid jargon unless asked.
                """;

            if (!includeContext)
                return basePrompt;

            var diaries = await _diaryRepository.GetByUserIdAsync(userId);
            var categories = await _categoryRepository.GetAllAsync();
            var catMap = categories.ToDictionary(c => c.Id, c => c.Name);

            var allTransactions = new List<(string Date, decimal Amount, string? Description, string? Category)>();

            foreach (var diary in diaries)
            {
                var transactions = await _transactionRepository.GetByDiaryIdAsync(diary.Id);
                foreach (var tx in transactions)
                {
                    allTransactions.Add((
                        tx.TransactionDate.ToString("yyyy-MM-dd"),
                        tx.Amount,
                        tx.Description,
                        tx.CategoryId.HasValue && catMap.TryGetValue(tx.CategoryId.Value, out var cat) ? cat : null
                    ));
                }
            }

            if (!allTransactions.Any())
                return basePrompt + "\n\nThe user has no transactions recorded yet.";

            var total = allTransactions.Sum(t => t.Amount);
            var expenses = allTransactions.Where(t => t.Amount < 0).Sum(t => t.Amount);
            var income = allTransactions.Where(t => t.Amount > 0).Sum(t => t.Amount);

            var byCategory = allTransactions
                .Where(t => t.Amount < 0 && t.Category != null)
                .GroupBy(t => t.Category!)
                .Select(g => $"{g.Key}: {g.Sum(t => t.Amount):N2}€")
                .ToList();

            var recentTx = allTransactions
                .OrderByDescending(t => t.Date)
                .Take(20)
                .Select(t => $"{t.Date} | {(t.Amount >= 0 ? "+" : "")}{t.Amount:N2}€ | {t.Description ?? "—"} | {t.Category ?? "Uncategorized"}");

            var context = $"""

                --- USER FINANCIAL SUMMARY ---
                Total balance across all diaries: {total:N2}€
                Total income: +{income:N2}€
                Total expenses: {expenses:N2}€

                Spending by category:
                {string.Join("\n", byCategory)}

                Recent transactions (up to 20):
                {string.Join("\n", recentTx)}
                --- END OF SUMMARY ---

                Use this data to give personalized, specific advice when relevant.
                """;

            return basePrompt + context;
        }
    }
}
