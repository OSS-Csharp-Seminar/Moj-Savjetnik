namespace MyAdvisor.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task ExecuteInTransactionAsync(Func<Task> operation);
    }
}
