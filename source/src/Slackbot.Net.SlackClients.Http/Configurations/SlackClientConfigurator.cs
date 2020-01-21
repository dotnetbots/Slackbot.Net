using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Slackbot.Net.SlackClients.Http.Configurations.Options;

namespace Slackbot.Net.SlackClients.Http.Configurations
{
    internal class SlackClientConfigurator : IConfigureNamedOptions<HttpClientFactoryOptions>
    {
        private readonly IOptions<BotTokenClientOptions> _botOptions;

        public SlackClientConfigurator(IOptions<BotTokenClientOptions> botOptions)
        {
            _botOptions = botOptions;
        }
        
        public void Configure(string name, HttpClientFactoryOptions options)
        {
            var token = "";

            if (name is nameof(SlackClient))
                token = _botOptions.Value.BotToken;

            if (name is nameof(SlackClient) && string.IsNullOrEmpty(token))
                throw new Exception("Missing token. Check configuration!");
            
            if (name is nameof(SlackClient))
            {
                options.HttpClientActions.Add(c =>
                {
                    c.BaseAddress = new Uri("https://slack.com/api/");
                    c.Timeout = TimeSpan.FromSeconds(15);
                    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                });
            }
            
            if (name is nameof(SlackOAuthAccessClient))
            {
                options.HttpClientActions.Add(c =>
                {
                    c.BaseAddress = new Uri("https://slack.com/api/");
                    c.Timeout = TimeSpan.FromSeconds(15);
                });
            }
        }

        public void Configure(HttpClientFactoryOptions options)
        {
           
        }
    }
}