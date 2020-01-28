using System;
using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Abstractions.Publishers;
using Slackbot.Net.SlackClients.Http.Configurations.Options;
using Slackbot.Net.SlackClients.Http.Extensions;

namespace Slackbot.Net.Extensions.Publishers.Slack
{
    public static class SlackbotWorkerBuilderExtensions
    {
        /// <summary>
        /// For distributed apps
        /// </summary>
        public static ISlackbotWorkerBuilder AddSlackPublisherBuilder(this ISlackbotWorkerBuilder builder)
        {
            builder.Services.AddSlackClientBuilder();
            builder.AddPublisherFactory<SlackPublisherBuilder>();
            return builder;
        }
        
        /// <summary>
        /// For standalone apps
        /// NB: Also registers ISlackClient for use via DI. No need to register SlackClient seperately
        /// </summary>
        public static ISlackbotWorkerBuilder AddSlackPublisher(this ISlackbotWorkerBuilder builder, Action<BotTokenClientOptions> config)
        {
            builder.Services.AddSlackHttpClient(config);
            builder.Services.AddSingleton<IPublisher, SlackPublisher>();
            return builder;
        }
    }
}