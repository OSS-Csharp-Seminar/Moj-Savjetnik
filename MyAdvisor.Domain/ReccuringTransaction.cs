namespace MyAdvisor.Domain
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
    }
}
