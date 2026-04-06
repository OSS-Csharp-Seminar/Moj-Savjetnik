using MyAdvisor.Application.DTOs.Transaction;
using MyAdvisor.Domain.Entities;
using MyAdvisor.Domain.Enums;
using Riok.Mapperly.Abstractions;

namespace MyAdvisor.Application.Mappers
{
    [Mapper]
    public partial class TransactionMapper
    {
        public partial TransactionDto ToDto(Transaction transaction);
        public partial Transaction ToEntity(AddTransactionRequestDto request);
    }
}
