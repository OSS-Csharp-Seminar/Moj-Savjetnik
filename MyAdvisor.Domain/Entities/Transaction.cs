namespace MyAdvisor.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; private set; }
        public int DiaryId { get; private set; }
        public int? CategoryId { get; private set; }
        public decimal Amount { get; private set; }
        public string? Description { get; private set; }
        public DateTime? TransactionDate { get; private set; }
        public string? PaymentMethod { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public FinancialDiary Diary { get; private set; }
        public Category? Category { get; private set; }
        private Transaction() { }
        public Transaction(
            int diaryId,
            decimal amount,
            int? categoryId = null,
            string? description = null,
            DateTime? transactionDate = null,
            string? paymentMethod = null)
        {
            if (diaryId <= 0)
                throw new ArgumentException("Invalid diaryId");

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero");

            DiaryId = diaryId;
            Amount = amount;
            CategoryId = categoryId;
            Description = description;
            TransactionDate = transactionDate ?? DateTime.UtcNow;
            PaymentMethod = paymentMethod;
            CreatedAt = DateTime.UtcNow;
        }
    }
}