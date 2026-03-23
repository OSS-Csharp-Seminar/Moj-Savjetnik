namespace MyAdvisor.Domain.Entities
{
    public class TransactionAiLog
    {
        public int Id { get; private set; }
        public int TransactionId { get; private set; }
        public string? RawOcrText { get; private set; }
        public int? AiCategoryId { get; private set; }
        public decimal? AiConfidence { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Transaction? Transaction { get; private set; }
        public Category? AiCategory { get; private set; }

        private TransactionAiLog() { }

        public TransactionAiLog(
            int transactionId,
            string? rawOcrText = null,
            int? aiCategoryId = null,
            decimal? aiConfidence = null)
        {
            if (transactionId <= 0)
                throw new ArgumentException("Invalid transactionId.", nameof(transactionId));

            if (aiConfidence.HasValue && (aiConfidence < 0 || aiConfidence > 1))
                throw new ArgumentException("AI confidence must be between 0 and 1.", nameof(aiConfidence));

            TransactionId = transactionId;
            RawOcrText = rawOcrText;
            AiCategoryId = aiCategoryId;
            AiConfidence = aiConfidence;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
