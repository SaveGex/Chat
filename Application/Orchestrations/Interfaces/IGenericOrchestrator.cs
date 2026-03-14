namespace Application.Orchestrations.Interfaces
{
    public interface IGenericOrchestrator<TImplementation>
        where TImplementation : class
    {
        Task<TResult> ExecuteAsync<TResult>(Func<TImplementation, Task<TResult>> action);
    }
}
