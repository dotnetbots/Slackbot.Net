using Slackbot.Net.Abstractions.Hosting;

namespace Slackbot.Net.Extensions.Samples.HelloWorld
{
    public static class SlackbotNetBuilderExtensions
    {
        public static ISlackbotWorkerBuilder AddSamples(this ISlackbotWorkerBuilder builder)
        {
            return builder
                .AddHandler<HelloWorldHandler>()
                .AddHandler<DebuggingStuffHandler>()
                .AddRecurring<WorkspacesAction>()
                .AddRecurring<HelloWorldRecurrer>()
                .BuildRecurrers();
        }
    }
}