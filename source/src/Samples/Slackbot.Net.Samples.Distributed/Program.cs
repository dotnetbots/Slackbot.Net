using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Extensions.Publishers.Slack;
using Slackbot.Net.Extensions.Samples.HelloWorld;

namespace Slackbot.Net.Samples.Distributed
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((c, s) =>
                {
                    s.AddSlackbotWorker<MyTokenStore>()
                        .AddSlackPublisherBuilder()
                        .AddSamples();
                })
                .ConfigureLogging(c =>
                {
                    c.AddFilter("Slackbot.Net.SlackClients.Http",level => level == LogLevel.Debug);
                    c.AddConsole().SetMinimumLevel(LogLevel.Trace);
                })
                .Build();

            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}