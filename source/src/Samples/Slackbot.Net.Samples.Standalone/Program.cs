using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                        .AddRecurringActions();
                })
                .ConfigureLogging(c =>
                {
                    c.AddConsole().SetMinimumLevel(LogLevel.Trace).AddFilter("Slackbot.Net.SlackClients.Http", LogLevel.Information);
                })
                .Build();

            using (host)
            {
                await host.RunAsync();
            }
        }
    }   
}