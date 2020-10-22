using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CronBackgroundServices.Extensions.Samples.HelloWorld;

namespace CronBackgroundServices.Samples.Distributed
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((c, s) =>
                {
                    s.AddRecurringActions().AddSamples();
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