using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slackbot.Net.SlackClients.Http.Configurations;
using Slackbot.Net.SlackClients.Http.Configurations.Options;

namespace Slackbot.Net.SlackClients.Http
{
    public class SlackClientBuilder : ISlackClientBuilder
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IOptions<BotTokenClientOptions> _options;

        public SlackClientBuilder(ILoggerFactory loggerFactory, IOptions<BotTokenClientOptions> options)
        {
            _loggerFactory = loggerFactory;
            _options = options;
        }

        public ISlackClient BuildFromConfiguration()
        {
            throw new System.NotImplementedException();
        }

        public ISlackClient Build(string token)
        {
            var c = new HttpClient();
            CommonHttpClientConfiguration.ConfigureHttpClient(c, token);
            return new SlackClient(c,_loggerFactory.CreateLogger<ISlackClient>());
        }
    }
}