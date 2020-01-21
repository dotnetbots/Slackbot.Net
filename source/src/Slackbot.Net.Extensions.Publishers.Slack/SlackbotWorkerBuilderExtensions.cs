using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.SlackClients.Http.Extensions;

namespace Slackbot.Net.Extensions.Publishers.Slack
{
    public static class SlackbotWorkerBuilderExtensions
    {
        public static ISlackbotWorkerBuilder AddSlackPublisherFactory(this ISlackbotWorkerBuilder builder)
        {
            builder.Services.AddSlackClientBuilder();
            builder.AddPublisherFactory<SlackPublisherBuilder>();
            return builder;
        }
    }
}