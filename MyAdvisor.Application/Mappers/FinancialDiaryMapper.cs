using MyAdvisor.Application.DTOs.FinancialDiary;
using MyAdvisor.Application.DTOs.Transaction;
using MyAdvisor.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace MyAdvisor.Application.Mappers
{
    [Mapper]
    public partial class FinancialDiaryMapper
    {
        public partial FinancialDiaryDto ToDto(FinancialDiary diary);
        public partial FinancialDiarySummaryDto ToSummaryDto(FinancialDiary diary);
        private partial TransactionDto MapTransaction(Transaction transaction);
    }
}
