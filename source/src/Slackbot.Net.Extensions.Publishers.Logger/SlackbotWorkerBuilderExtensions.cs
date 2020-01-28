using Slackbot.Net.Abstractions.Hosting;

namespace Slackbot.Net.Extensions.Publishers.Logger
{
    public static class SlackbotWorkerBuilderExtensions
    {
        public static ISlackbotWorkerBuilder AddLoggerPublisherBuilder(this ISlackbotWorkerBuilder builder)
        {
            builder.AddPublisherFactory<LoggerPublisherBuilder>();
            return builder;
        }
    }
}