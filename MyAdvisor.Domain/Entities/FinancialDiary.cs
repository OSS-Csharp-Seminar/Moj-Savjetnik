namespace MyAdvisor.Domain.Entities
{
    public class FinancialDiary
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public DateTime Date { get; private set; }
        public decimal? TotalAmount { get; private set; }
        public string? Notes { get; private set; }
        public User User { get; private set; }
        public List<Transaction> Transactions { get; private set; } = new();
        private FinancialDiary() { }
        public FinancialDiary(int userId, DateTime date, string? notes = null)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid userId");

            UserId = userId;
            Date = date.Date;
            Notes = notes;
        }
    }
}