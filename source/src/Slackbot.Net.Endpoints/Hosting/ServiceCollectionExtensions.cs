using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Endpoints.Abstractions;

namespace Slackbot.Net.Endpoints.Hosting
{
    public static class ServiceCollectionExtensions
    {
        public static ISlackbotEndpointsBuilder AddSlackbotInteractiveHandlers(this IServiceCollection services)
        {
            var builder = new SlackbotInteractiveEndpointsBuilder(services);
            builder.AddSlackbotInterctiveDependencies();
            return builder;
        }
        
        public static ISlackbotEventHandlersBuilder AddSlackBotEvents<T>(this IServiceCollection services) where T: class, ITokenStore
        {
            services.AddSingleton<ITokenStore, T>();
            
            services.AddSingleton<ISelectEventHandlers, EventHandlerSelector>();
            return new SlackBotEventHandlersBuilder(services);
        }
    }
}