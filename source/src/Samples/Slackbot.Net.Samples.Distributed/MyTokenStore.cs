using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Hosting;

namespace Slackbot.Net.Samples.Distributed
{
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