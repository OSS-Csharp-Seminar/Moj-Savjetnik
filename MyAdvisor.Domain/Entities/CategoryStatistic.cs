namespace MyAdvisor.Domain.Entities
{
    public class CategoryStatistic
    {
        public int Id { get; private set; }
        public int StatisticsId { get; private set; }
        public int CategoryId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public SpendingStatistic? Statistics { get; private set; }
        public Category? Category { get; private set; }

        private CategoryStatistic() { }

        public CategoryStatistic(int statisticsId, int categoryId, decimal totalAmount)
        {
            if (statisticsId <= 0)
                throw new ArgumentException("Invalid statisticsId.", nameof(statisticsId));

            if (categoryId <= 0)
                throw new ArgumentException("Invalid categoryId.", nameof(categoryId));

            if (totalAmount < 0)
                throw new ArgumentException("TotalAmount cannot be negative.", nameof(totalAmount));

            StatisticsId = statisticsId;
            CategoryId = categoryId;
            TotalAmount = totalAmount;
        }

        public void AddAmount(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            TotalAmount += amount;
        }
    }
}
