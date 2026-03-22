namespace MyAdvisor.Domain
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
    }
}