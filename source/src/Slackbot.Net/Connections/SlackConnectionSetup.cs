using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Handlers;
using Slackbot.Net.SlackClients.Rtm;
using Slackbot.Net.SlackClients.Rtm.Configurations;
using Slackbot.Net.SlackClients.Rtm.Exceptions;

namespace Slackbot.Net.Connections
{
    internal class SlackConnectionSetup
    {
        private readonly IServiceProvider _services;
        private readonly Dictionary<string,IConnection> _connections;
        private readonly ITokenStore _tokenStore;
        private readonly ILogger<SlackConnectionSetup> _logger;
        private readonly ILogger<Connector> _loggerConnector;

        public SlackConnectionSetup(IServiceProvider services)
        {
            _services = services;
            var loggerFactory = _services.GetService<ILoggerFactory>();
            _connections = new Dictionary<string, IConnection>();
            _tokenStore = _services.GetService<ITokenStore>();
            _logger = loggerFactory.CreateLogger<SlackConnectionSetup>();
            _loggerConnector = loggerFactory.CreateLogger<Connector>();
        }

        public async Task TryConnectWorkspaces()
        {
            var tokens = await _tokenStore.GetTokens();

            if (tokens == null || !tokens.Any())
            {
                _logger.LogInformation("No tokens returned from token store. Skipping connects.");
            }

            foreach (var token in tokens)
            {
                var existingConnection = _connections.Values.FirstOrDefault(w => w.SlackKey == token);
                if (existingConnection == null)
                {
                    _logger.LogInformation("Connecting..");
                    await Connect(token);
                }
                else
                {
                    _logger.LogTrace($"Token in use for {existingConnection.Team?.Name}");
                }
            }
        }

        private async Task Connect(string token)
        {

            var slackConnector = new Connector(new RtmOptions
            {
                Token = token
            },_loggerConnector);

            var handlerSelector = _services.GetService<HandlerSelector>();
            IConnection connection;
            try
            {
                connection = await slackConnector.Connect();
            }
            catch (TokenRevokedException tre)
            {
                _logger.LogWarning($"Token was revoked, and will be deleted. {LastSectionOf(token)}\n{tre.Message}");
                await _tokenStore.Delete(token);
                _logger.LogInformation($"Token deleted. {LastSectionOf(token)}");
                return;
            }
            catch (MissingScopeException mse)
            {
                _logger.LogWarning($"Token was lacking scopes, and will be deleted. {LastSectionOf(token)}\n{mse.Message}");
                await _tokenStore.Delete(token);
                _logger.LogInformation($"Token deleted. {LastSectionOf(token)}");
                return;
            }
            catch (HandshakeException he)
            {
                _logger.LogError($"Could not connect using token ending in {LastSectionOf(token)}\n{he.Message}");
                return;
            }

            connection.OnDisconnect += (teamId, teamName) =>
            {
                Log("disconnect", connection.Team?.Name);
                _connections.Remove(connection.Team.Id);
            };

            connection.OnReconnecting += (teamId, teamName) => Log("reconnecting", teamName);
            
            connection.OnReconnect += (teamId, teamName) =>
            {
                Log("reconnect", teamName);
                if(!_connections.ContainsKey(teamId))
                    _connections.Add(teamId, connection);
                
                _logger.LogInformation($"Connected to workspace {connection.Team?.Name}");
                return Task.CompletedTask;
            };
            
            connection.OnReconnectFailure += async (failure, teamId, teamName) =>
            {
                if (failure == "token_revoked" || failure == "account_inactive")
                {
                    _logger.LogWarning($"OnReconnectFailure: Token was revoked, and will be deleted. {LastSectionOf(token)}\n{failure}");
                    await _tokenStore.Delete(token);
                    _logger.LogInformation($"OnReconnectFailure: Token deleted. {LastSectionOf(token)}");
                    _connections.Remove(teamId);
                }
            };
            
            connection.OnMessageReceived += msg => handlerSelector.HandleIncomingMessage(SlackConnectorMapper.Map(msg));

            _connections.Add(connection.Team.Id, connection);
        }

        private Task Log(string action, string teamName)
        {
            _logger.LogWarning($"{action} '{teamName}'");
            return Task.CompletedTask;
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