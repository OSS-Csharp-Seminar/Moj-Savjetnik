namespace MyAdvisor.Domain
{
    public class TransactionAiLog
    {
        public int Id { get; private set; }
        public int TransactionId { get; private set; }

        public string? RawOcrText { get; private set; }
        public int? AiCategoryId { get; private set; }
        public decimal? AiConfidence { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Transaction Transaction { get; private set; }
        public Category? AiCategory { get; private set; }
    }
}
