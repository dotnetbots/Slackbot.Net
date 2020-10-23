using Microsoft.Extensions.DependencyInjection;

namespace CronBackgroundServices
{
    public interface IRecurringActionsBuilder
    {
        IServiceCollection Services { get; }
        IRecurringActionsBuilder AddRecurrer<T>() where T : class, IRecurringAction;
        
        IRecurringActionsBuilder Build();

    }
}