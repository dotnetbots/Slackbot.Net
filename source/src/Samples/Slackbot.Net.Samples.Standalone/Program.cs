using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CronBackgroundServices.Extensions.Samples.HelloWorld;

namespace CronBackgroundServices.Samples.Standalone
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((c,s) =>
                {
                    s.AddRecurringActions().AddSamples();
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