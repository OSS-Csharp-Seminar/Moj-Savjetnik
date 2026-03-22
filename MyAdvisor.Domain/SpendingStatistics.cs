using System;
using System.Collections.Generic;
using System.Text;

namespace MyAdvisor.Domain
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
    }
}
