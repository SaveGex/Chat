using Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orchestrations.Interfaces
{
    public interface IGenericOrchestrator<TImplementation>
        where TImplementation : class
    {
        Task<TResult> ExecuteAsync<TResult>(Func<TImplementation, Task<TResult>> action);
    }
}
