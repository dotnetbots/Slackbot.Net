using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        
        public static ISlackbotWorkerBuilder AddWorkspaceService<T>(this ISlackbotWorkerBuilder builder) where T: class, ITokenStore
        {
            builder.Services.Remove<ITokenStore>();
            builder.Services.AddSingleton<ITokenStore, T>();
            return builder;
        }
        
        private static void Remove<T>(this IServiceCollection services) where T : class
        {
            var serviceDescriptors = services.Where(descriptor => descriptor.ServiceType == typeof(T)).ToList();
            foreach (var service in serviceDescriptors)
            {
                var t = services.Remove(service);
            }
        }
    }
}