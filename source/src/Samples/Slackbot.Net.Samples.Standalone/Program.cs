using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Configuration;
using Slackbot.Net.Extensions.Publishers.Slack;
using Slackbot.Net.Extensions.Samples.HelloWorld;

namespace Slackbot.Net.Samples.Standalone
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((c,s) =>
                {
                    s.AddSlackbotWorker(c.Configuration)
                        
                        .AddSlackPublisherFactory()
                        .AddSamples();
                })
                .ConfigureLogging(c => c.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .Build();

            using (host)
            {
                await host.RunAsync();
            }
        }
    }   
}