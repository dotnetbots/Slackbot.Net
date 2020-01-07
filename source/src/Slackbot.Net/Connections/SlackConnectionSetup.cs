using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slackbot.Net.Abstractions.Handlers;
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
            var logger = _services.GetService<ILogger<Connector>>();
            var slackConnector = new Connector(new RtmOptions
            {
                ApiKey = options.Value.Slackbot_SlackApiKey_BotUser
            });
            
            var handlerSelector = _services.GetService<HandlerSelector>();
            Connection = await slackConnector.Connect();
            Connection.OnMessageReceived += msg => handlerSelector.HandleIncomingMessage(SlackConnectorMapper.Map(msg));
            
            if (Connection.IsConnected)
                logger.LogInformation("Connected");

        }

        private IConnection Connection { get; set; }

        public BotDetails GetBotDetails()
        {
            return new BotDetails
            {
                Id = Connection.Self.Id,
                Name = Connection.Self.Name
            };
        }
    }
}