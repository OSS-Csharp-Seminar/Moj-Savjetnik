namespace MyAdvisor.Application.Interfaces.Contracts
{
    public interface IUnitOfWork
    {
        Task ExecuteInTransactionAsync(Func<Task> operation);
    }
}
