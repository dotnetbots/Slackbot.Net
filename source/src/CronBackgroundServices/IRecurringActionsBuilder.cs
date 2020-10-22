using CronBackgroundServices.Abstractions.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace CronBackgroundServices.Abstractions.Hosting
{
    public interface IRecurringActionsBuilder
    {
        IServiceCollection Services { get; }
        IRecurringActionsBuilder AddRecurrer<T>() where T : class, IRecurringAction;
        
        IRecurringActionsBuilder Build();

    }
}