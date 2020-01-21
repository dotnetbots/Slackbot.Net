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
        private readonly List<Workspace> _tokens;

        public MyTokenStore()
        {
            _tokens = new List<Workspace>()
            {
                new Workspace { 
                    Token = Environment.GetEnvironmentVariable("Slackbot_SlackApiKey_BotUser"),
                    TeamId = "T0EC3DG3A"
                },
                new Workspace { 
                    Token = Environment.GetEnvironmentVariable("FplBot_SlackApiKey_BotUser"),
                    TeamId = "T0A9QSU83"
                }
            };
        }
        
        public Task<IEnumerable<string>> GetTokens()
        {
            return Task.FromResult(_tokens.Select(c => c.Token));
        }

        public Task<string> GetTokenByTeamId(string teamId)
        {
            return Task.FromResult(_tokens.First(t => t.TeamId == teamId).Token);
        }

        private class Workspace
        {
            public string Token { get; set; }
            public string TeamId { get; set; }
        }
    }
}