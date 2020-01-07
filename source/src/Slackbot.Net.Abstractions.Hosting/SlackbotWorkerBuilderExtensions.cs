using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Publishers;

namespace Slackbot.Net.Abstractions.Hosting
{
    public static class SlackbotWorkerBuilderExtensions
    {
        public static ISlackbotWorkerBuilder AddHandler<T>(this ISlackbotWorkerBuilder builder) where T : class, IHandleMessages
        {
            builder.Services.AddSingleton<IHandleMessages, T>();
            return builder;
        }

        public static ISlackbotWorkerBuilder AddPublisher<T>(this ISlackbotWorkerBuilder builder) where T: class, IPublisher
        {
            builder.Services.AddSingleton<IPublisher, T>();
            return builder;
        }
        
        public static ISlackbotWorkerBuilder AddRecurring<T>(this ISlackbotWorkerBuilder builder) where T: class, IRecurringAction
        {
            builder.Services.AddSingleton<IRecurringAction, T>();
            return builder;
        }
    }
}