using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Abstractions.Publishers;
using Slackbot.Net.Extensions.Publishers.Logger;

namespace Slackbot.Net.Extensions.KitchenSink
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((c,s) =>
                {
                    s.AddSlackbotWorker(c.Configuration)
                        .AddPublisher<LoggerPublisher>()
                        .AddHandler<HelloWorldHandler>()
                        .AddRecurring<HelloWorldRecurrer>()
                        .BuildRecurrers();
                })
                .ConfigureLogging(c => c.AddConsole())
                .Build();

            using (host)
            {
                await host.RunAsync();
            }
        }
    }

    internal class HelloWorldHandler : IHandleMessages
    {
        private readonly IPublisher _publisher;
        public bool ShouldShowInHelp { get; }
        public Tuple<string, string> GetHelpDescription() => new Tuple<string, string>("hw", "hw");

        public HelloWorldHandler(IPublisher publisher)
        {
            _publisher = publisher;
        }
        
        public async Task<HandleResponse> Handle(SlackMessage message)
        {
            await _publisher.Publish(new Notification
            {
                Msg = $"Hello world, {message.User.Name} ({message.User.FirstName} {message.User.LastName})\n" +
                      $"I am {message.Bot.Name}"
            });
            return new HandleResponse("Responded");
        }

        public bool ShouldHandle(SlackMessage message)
        {
            return message.MentionsBot && message.Text.Contains("hw");
        }
    }

    internal class HelloWorldRecurrer : IRecurringAction
    {
        private readonly IPublisher _publisher;

        public HelloWorldRecurrer(IPublisher publisher)
        {
            _publisher = publisher;
        }
        
        public async Task Process()
        {
            await _publisher.Publish(new Notification
            {
                Msg = $"Hi"
            });
        }

        public string Cron { get; } = "*/10 * * * * *";
    }
}