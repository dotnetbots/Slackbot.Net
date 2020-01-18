using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Extensions.Publishers.Logger;

namespace Slackbot.Net.Extensions.Samples.HelloWorld
{
    public static class SlackbotNetBuilderExtensions
    {
        public static ISlackbotWorkerBuilder AddSamples(this ISlackbotWorkerBuilder builder)
        {
            return builder
                .AddPublisher<LoggerPublisher>()
                .AddHandler<HelloWorldHandler>()
                .AddRecurring<HelloWorldRecurrer>()
                .BuildRecurrers();
        }
    }
}