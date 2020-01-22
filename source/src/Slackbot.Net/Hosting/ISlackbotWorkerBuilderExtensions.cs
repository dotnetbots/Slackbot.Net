using System;
using Microsoft.Extensions.Configuration;
using Slackbot.Net;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Abstractions.Publishers;
using Slackbot.Net.Configuration;
using Slackbot.Net.Connections;
using Slackbot.Net.Dynamic;
using Slackbot.Net.Handlers;
using Slackbot.Net.Hosting;
using Slackbot.Net.SlackClients.Http;
using Slackbot.Net.SlackClients.Http.Extensions;
using Slackbot.Net.Validations;

// namespace on purpose:
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class SlackbotWorkerBuilderExtensions
    {
        /// <summary>
        /// For distributed apps
        /// </summary>
        public static ISlackbotWorkerBuilder AddSlackbotWorker<T>(this IServiceCollection services) where T: class, ITokenStore
        {
            services.Configure<SlackOptions>(o => {});
            services.AddSingleton<ITokenStore, T>();
            var builder = new SlackbotWorkerBuilder(services);
            builder.AddRtmConnections();
            return builder;
        }
        
        /// <summary>
        /// For single workspace apps using a pre-configured token 
        /// </summary>
        public static ISlackbotWorkerBuilder AddSlackbotWorker(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureAndValidate<SlackOptions>(configuration);
            services.AddSingleton<ITokenStore, ConfigurationTokenStore>();
            var builder = new SlackbotWorkerBuilder(services);
            builder.AddRtmConnections();
            return builder;
        }

        public static ISlackbotWorkerBuilder AddSlackbotWorker(this IServiceCollection services, Action<SlackOptions> action)
        {
            services.ConfigureAndValidate(action);
            services.AddSingleton<ITokenStore, ConfigurationTokenStore>();
            var builder = new SlackbotWorkerBuilder(services);
            builder.AddRtmConnections();
            return builder;
        }

        private static ISlackbotWorkerBuilder AddRtmConnections(this ISlackbotWorkerBuilder builder)
        {
            builder.Services.AddSingleton<ISlackClientService, SlackClientService>();
            builder.Services.AddSlackClientBuilder();
            builder.Services.AddSingleton<SlackConnectionSetup>();
            builder.Services.AddSingleton<HandlerSelector>();
            builder.Services.AddHostedService<SlackRtmConnectionHostedService>();
            return builder;
        }
    }
}