namespace MyAdvisor.Domain.Entities
{
    public class SpendingStatistic
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
        public decimal TotalSpent { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public User User { get; private set; }
        public List<CategoryStatistic> CategoryBreakdown { get; private set; } = new();
        private SpendingStatistic() { }
        public SpendingStatistic(int userId, int month, int year)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid userId");

            if (month < 1 || month > 12)
                throw new ArgumentException("Month must be between 1 and 12");

            if (year < 2000) 
                throw new ArgumentException("Invalid year");

            UserId = userId;
            Month = month;
            Year = year;
            CreatedAt = DateTime.UtcNow;
            TotalSpent = 0;
        }
    }
}
