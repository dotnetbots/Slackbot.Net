using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Handlers;
using Slackbot.Net.SlackClients.Rtm;
using Slackbot.Net.SlackClients.Rtm.Configurations;
using Slackbot.Net.SlackClients.Rtm.Exceptions;

namespace Slackbot.Net.Connections
{
    internal class SlackConnectionSetup
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<Connector> _logger;
        private readonly Dictionary<string,ConnectedWorkspace> _connectedWorkspaces;
        private readonly ITokenStore _tokenStore;

        public SlackConnectionSetup(IServiceProvider services)
        {
            _services = services;
            _logger = _services.GetService<ILogger<Connector>>();
            _connectedWorkspaces = new Dictionary<string, ConnectedWorkspace>();
            _tokenStore = _services.GetService<ITokenStore>();
        }
        
        public async Task TryConnectWorkspaces()
        {
            var tokens = await _tokenStore.GetTokens();
        
            foreach (var token in tokens)
            {
                var existingConnection = _connectedWorkspaces.Values.FirstOrDefault(w => w.Token == token);
                if (existingConnection == null)
                {
                    _logger.LogInformation("Connecting..");
                    await Connect(token);
                }
                else
                {
                    _logger.LogTrace($"Token in use for {existingConnection.TeamName}");
                }
            }
        }
        
        private async Task<IConnection> Connect(string token)
        {
            var slackConnector = new Connector(new RtmOptions
            {
                Token = token
            });

            var handlerSelector = _services.GetService<HandlerSelector>();
            IConnection connection = null;
            try
            {
                connection = await slackConnector.Connect();
            }
            catch (HandshakeException he)
            {
                _logger.LogError($"Could not connect using token ending in {LastSectionOf(token)}\n{he.Message}");
                return connection;
            }

            connection.OnMessageReceived += msg => handlerSelector.HandleIncomingMessage(SlackConnectorMapper.Map(msg));
            var conn = connection;
            var workspace = new ConnectedWorkspace
            {
                Token = token, 
                TeamId = conn.Team.Id, 
                TeamName = conn.Team.Name
            };
            _connectedWorkspaces.Add(workspace.TeamId, workspace);
           
            if (conn.IsConnected)
                _logger.LogInformation($"Connected to workspace {workspace.TeamName}");
           
            return conn;
        }

        private string LastSectionOf(string token)
        {
            if (token.Length > 5)
            {
                var last5 = token.Substring(token.Length - 5);
                var stars = new string('*', token.Length - 5);
                return $"{stars}{last5}";
            }

            return token;

        }
    }
}