using System.Net.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.SlackClients.Http.Configurations;

namespace Slackbot.Net.SlackClients.Http
{
    public class SlackClientBuilder : ISlackClientBuilder
    {
        private readonly ILoggerFactory _loggerFactory;

        public SlackClientBuilder(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }
        
        public ISlackClient Build(string token)
        {
            var c = new HttpClient();
            CommonHttpClientConfiguration.ConfigureHttpClient(c, token);
            return new SlackClient(c,_loggerFactory.CreateLogger<ISlackClient>());
        }
    }
}