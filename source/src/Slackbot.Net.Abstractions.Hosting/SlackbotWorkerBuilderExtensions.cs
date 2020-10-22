using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Abstractions.Handlers;

namespace Slackbot.Net.Abstractions.Hosting
{
    public static class SlackbotWorkerBuilderExtensions
    {
        public static ISlackbotWorkerBuilder AddRecurring<T>(this ISlackbotWorkerBuilder builder) where T: class, IRecurringAction
        {
            builder.Services.AddSingleton<IRecurringAction, T>();
            return builder;
        }
    }
}