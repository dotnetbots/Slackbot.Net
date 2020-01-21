using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Handlers;
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
                    s.AddSlackbotWorker<MyTokenStore>().AddSamples();
                })
                .ConfigureLogging(c => c.AddConsole())
                .Build();

            using (host)
            {
                await host.RunAsync();
            }
        }
    }

    internal class MyTokenStore : ITokenStore
    {
        private readonly List<string> _tokens;

        public MyTokenStore()
        {
            _tokens = new List<string>()
            {
                Environment.GetEnvironmentVariable("Slackbot_SlackApiKey_BotUser"),
                Environment.GetEnvironmentVariable("Slackbot_SlackApiKey_BotUser"),
                Environment.GetEnvironmentVariable("Slackbot_SlackApiKey_BotUser"),
                Environment.GetEnvironmentVariable("FplBot_SlackApiKey_BotUser"),
            };
        }
        
        public Task<IEnumerable<string>> GetTokens()
        {
            return Task.FromResult(_tokens.AsEnumerable());
        }
    }
}