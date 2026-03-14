using Application.Orchestrations.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Orchestrations
{
    public class GenericOrchestrator<TImplementation> : IGenericOrchestrator<TImplementation>
        where TImplementation : class
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public GenericOrchestrator(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public Task<TResult> ExecuteAsync<TResult>(Func<TImplementation, Task<TResult>> action)
        {
            var scope = serviceScopeFactory.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<TImplementation>();

            return action(service);
        }
    }
}
