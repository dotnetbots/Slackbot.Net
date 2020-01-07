using Slackbot.Net.Endpoints.Hosting;
using Slackbot.Net.Endpoints.Interactive;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ISlackbotEndpointsBuilderExtensions
    {
        public static ISlackbotEndpointsBuilder AddSlackbotEndpoints(this IServiceCollection services)
        {
            services.AddRouting();
            var builder = new SlackbotEndpointsBuilder(services);
            builder.AddSlackbotEndpointsDependencies();
            return builder;
        }

        public static ISlackbotEndpointsBuilder AddEndpointHandler<T>(this ISlackbotEndpointsBuilder builder) where T: class, IHandleInteractiveActions
        {
            builder.Services.AddSingleton<IHandleInteractiveActions, T>();
            return builder;
        }

        internal static void AddSlackbotEndpointsDependencies(this ISlackbotEndpointsBuilder builder)
        {
            builder.Services.AddSingleton<IRespond, Responder>();
        }
    }
}