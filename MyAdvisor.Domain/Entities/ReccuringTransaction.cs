namespace MyAdvisor.Domain.Entities
{
    public class RecurringTransaction
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int CategoryId { get; private set; }
        public decimal? Amount { get; private set; }
        public string? Frequency { get; private set; }
        public DateTime? NextDueDate { get; private set; }
        public string? Description { get; private set; }
        public User User { get; private set; }
        public Category Category { get; private set; }
        private RecurringTransaction() { }
        public RecurringTransaction(
            int userId,
            int categoryId,
            decimal amount,
            string frequency,
            DateTime? nextDueDate = null,
            string? description = null)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid userId");

            if (categoryId <= 0)
                throw new ArgumentException("Invalid categoryId");

            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative");

            if (string.IsNullOrWhiteSpace(frequency))
                throw new ArgumentException("Frequency is required");

            UserId = userId;
            CategoryId = categoryId;
            Amount = amount;
            Frequency = frequency;
            NextDueDate = nextDueDate;
            Description = description;
        }
    }
}
