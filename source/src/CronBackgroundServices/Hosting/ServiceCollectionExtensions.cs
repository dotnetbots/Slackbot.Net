using CronBackgroundServices;
using CronBackgroundServices.Hosting;

// namespace on purpose:
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class SlackbotWorkerBuilderExtensions
    {
        /// <summary>
        /// For distributed apps
        /// </summary>
        public static IRecurringActionsBuilder AddRecurringActions(this IServiceCollection services)
        {
            return new RecurringActionsBuilder(services);
        }
    }
}