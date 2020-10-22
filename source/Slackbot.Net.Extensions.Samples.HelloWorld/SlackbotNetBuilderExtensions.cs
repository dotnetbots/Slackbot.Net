using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Endpoints.Hosting;

namespace Slackbot.Net.Extensions.Samples.HelloWorld
{
    public static class SlackbotNetBuilderExtensions
    {
        public static ISlackbotHandlersBuilder AddAppMentionHandlerSamples(this ISlackbotHandlersBuilder builder)
        {
            return builder.AddHandler<HelloWorldHandler>();
        }

        public static ISlackbotWorkerBuilder AddRecurringActions(this ISlackbotWorkerBuilder builder)
        {
            return builder
                .AddRecurring<WorkspacesAction>()
                .AddRecurring<HelloWorldRecurrer>()
                .BuildRecurrers();
        }
    }
}