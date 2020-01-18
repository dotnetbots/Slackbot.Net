using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slackbot.Net.Configuration;
using Slackbot.Net.Handlers;
using Slackbot.Net.SlackClients.Rtm;
using Slackbot.Net.SlackClients.Rtm.Configurations;

namespace Slackbot.Net.Connections
{
    internal class SlackConnectionSetup
    {
        private readonly IServiceProvider _services;

        public SlackConnectionSetup(IServiceProvider services)
        {
            _services = services;
        }

        public async Task Connect()
        { 
            var options = _services.GetService<IOptions<SlackOptions>>();
            await Connect(options.Value.Slackbot_SlackApiKey_BotUser);
        }

        public async Task<IConnection> Connect(string apiKey)
        {
            var logger = _services.GetService<ILogger<Connector>>();
            var slackConnector = new Connector(new RtmOptions
            {
                ApiKey = apiKey
            });
            
            var handlerSelector = _services.GetService<HandlerSelector>();
            var connection = await slackConnector.Connect();
            connection.OnMessageReceived += msg => handlerSelector.HandleIncomingMessage(SlackConnectorMapper.Map(msg));

            if (connection.IsConnected)
                logger.LogInformation("Connected");
            
            return connection;
        }
    }
}