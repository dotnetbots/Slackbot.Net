using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slackbot.Net.SlackClients.Http;
using Slackbot.Net.SlackClients.Http.Extensions;
using Xunit.Abstractions;

namespace CronBackgroundServices.Tests
{
    public class Setup
    {
        protected readonly ISearchClient SearchClient;
        protected readonly ISlackClient SlackClient;
        protected readonly ISlackOAuthAccessClient SlackOAuthClient;
        protected string Channel;
        protected string Text;

        public Setup(ITestOutputHelper helper)
        {
            var services = new ServiceCollection();
            services.AddSlackbotOauthClient(c =>
            {
                c.OauthToken = Environment.GetEnvironmentVariable("Slackbot_SlackApiKey_SlackApp");
            });
            
            services.AddSlackHttpClient(c =>
            {
                c.BotToken = Environment.GetEnvironmentVariable("Slackbot_SlackApiKey_BotUser");
            });

            services.AddSlackbotOauthAccessHttpClient();

            services.AddSingleton<ILogger<ISlackClient>>(new XUnitLogger<ISlackClient>(helper));
            
            var provider = services.BuildServiceProvider();
            SearchClient = provider.GetService<ISearchClient>();
            SlackClient = provider.GetService<ISlackClient>();
            SlackOAuthClient = provider.GetService<ISlackOAuthAccessClient>();
            Channel = "#testss";
            Text = "Test";
        }
    }
}