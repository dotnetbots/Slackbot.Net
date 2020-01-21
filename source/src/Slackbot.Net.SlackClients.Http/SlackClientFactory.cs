using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace Slackbot.Net.SlackClients.Http
{
    public class SlackClientFactory : ISlackClientFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public SlackClientFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }
        
        public ISlackClient Create(string token)
        {
            var c = new HttpClient
            {
                BaseAddress = new Uri("https://slack.com/api/"), 
                Timeout = TimeSpan.FromSeconds(15)
            };
            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return new SlackClient(c,_loggerFactory.CreateLogger<ISlackClient>());
        }
    }

    public interface ISlackClientFactory
    {
        ISlackClient Create(string token);
    }
}