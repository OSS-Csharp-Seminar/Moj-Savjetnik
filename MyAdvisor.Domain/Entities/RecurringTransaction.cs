using MyAdvisor.Domain.Enums;

namespace MyAdvisor.Domain.Entities
{
    public class RecurringTransaction
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int CategoryId { get; private set; }
        public decimal Amount { get; private set; }
        public Frequency Frequency { get; private set; }
        public DateTime? NextDueDate { get; private set; }
        public string? Description { get; private set; }
        public User? User { get; private set; }
        public Category? Category { get; private set; }

        private RecurringTransaction() { } // For EF Core

        public RecurringTransaction(
            int userId,
            int categoryId,
            decimal amount,
            Frequency frequency,
            DateTime? nextDueDate = null,
            string? description = null)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid userId.", nameof(userId));

            if (categoryId <= 0)
                throw new ArgumentException("Invalid categoryId.", nameof(categoryId));

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

UserId = userId;
            CategoryId = categoryId;
            Amount = amount;
            Frequency = frequency;
            NextDueDate = nextDueDate;
            Description = description;
        }

        public void AdvanceDueDate(DateTime nextDueDate) => NextDueDate = nextDueDate;
    }
}
