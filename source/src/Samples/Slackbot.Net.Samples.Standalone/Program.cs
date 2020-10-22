using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Configuration;
using Slackbot.Net.Extensions.Publishers.Logger;
using Slackbot.Net.Extensions.Publishers.Slack;
using Slackbot.Net.Extensions.Samples.HelloWorld;
using Slackbot.Net.SlackClients.Http;

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
                        .AddSamples();
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