using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Abstractions.Publishers;
using Slackbot.Net.Connections;
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

    internal class HelloWorldRecurrer : IRecurringAction
    {
        private readonly IPublisher _publisher;
        private readonly IGetConnectionDetails _connDetailsFetcher;

        public HelloWorldRecurrer(IPublisher publisher, IGetConnectionDetails connDetailsFetcher)
        {
            _publisher = publisher;
            _connDetailsFetcher = connDetailsFetcher;
        }
        
        public async Task Process()
        {
            var botDetails = _connDetailsFetcher.GetConnectionBotDetails();
            
            await _publisher.Publish(new Notification
            {
                Msg = $"{botDetails.Name}|{botDetails.Id}"
            });
        }

        public string Cron { get; } = "*/1 * * * * *";
    }
}