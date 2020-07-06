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
        private readonly IHttpClientFactory _factory;
        private readonly IOptions<BotTokenClientOptions> _options;

        public SlackClientBuilder(ILoggerFactory loggerFactory, IHttpClientFactory factory, IOptions<BotTokenClientOptions> options)
        {
            _loggerFactory = loggerFactory;
            _factory = factory;
            _options = options;
        }

        public ISlackClient Build(string token)
        {
            var c = _factory.CreateClient();
            CommonHttpClientConfiguration.ConfigureHttpClient(c, token);
            return new SlackClient(c,_loggerFactory.CreateLogger<ISlackClient>());
        }
    }
}