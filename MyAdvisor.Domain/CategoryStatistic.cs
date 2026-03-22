using System;
using System.Collections.Generic;
using System.Text;

namespace MyAdvisor.Domain
{
    public class CategoryStatistic
    {
        public int Id { get; private set; }
        public int StatisticsId { get; private set; }
        public int CategoryId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public SpendingStatistic Statistics { get; private set; }
        public Category Category { get; private set; }
    }
}
