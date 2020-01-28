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
        public static ISlackbotWorkerBuilder AddSlackPublisherBuilder(this ISlackbotWorkerBuilder builder)
        {
            builder.Services.AddSlackClientBuilder();
            builder.AddPublisherFactory<SlackPublisherBuilder>();
            return builder;
        }
        
        public static ISlackbotWorkerBuilder AddSlackPublisher(this ISlackbotWorkerBuilder builder, Action<BotTokenClientOptions> config)
        {
            builder.Services.AddSlackHttpClient(config);
            builder.Services.AddSingleton<IPublisher, SlackPublisher>();
            return builder;
        }
    }
}