using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Configuration;

namespace Slackbot.Net
{
    internal class ConfigurationTokenStore : ITokenStore
    {
        private string _token;

        public ConfigurationTokenStore(IOptions<SlackOptions> options)
        {
            _token = options.Value.Slackbot_SlackApiKey_BotUser;
        }


        public Task<IEnumerable<string>> GetTokens()
        {
            var strings = new List<string> { _token };
            return Task.FromResult(strings.AsEnumerable());
        }

    }
}