namespace MyAdvisor.Domain.Entities
{
    public class FinancialDiary
    {
        private readonly List<Transaction> _transactions = new();

        public int Id { get; private set; }
        public int UserId { get; private set; }
        public DateTime Date { get; private set; }
        public decimal TotalAmount { get; private set; }
        public string? Notes { get; private set; }
        public User? User { get; private set; }
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        private FinancialDiary() { }

        public FinancialDiary(int userId, DateTime date, string? notes = null)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid userId.", nameof(userId));

            UserId = userId;
            Date = date.Date;
            Notes = notes;
        }

        public void UpdateNotes(string? notes) => Notes = notes;

        public void RecalculateTotalAmount()
            => TotalAmount = _transactions.Sum(t => t.Amount);
    }
}
