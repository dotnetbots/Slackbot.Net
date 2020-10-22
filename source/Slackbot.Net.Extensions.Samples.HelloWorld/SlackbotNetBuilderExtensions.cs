using CronBackgroundServices.Abstractions.Hosting;
using Slackbot.Net.Endpoints.Hosting;

namespace CronBackgroundServices.Extensions.Samples.HelloWorld
{
    public static class SlackbotNetBuilderExtensions
    {
        public static ISlackbotHandlersBuilder AddAppMentionHandlerSamples(this ISlackbotHandlersBuilder builder)
        {
            return builder.AddHandler<HelloWorldHandler>();
        }
    }
}