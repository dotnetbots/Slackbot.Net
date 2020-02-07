using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slackbot.Net.SlackClients.Rtm.Configurations;
using Slackbot.Net.SlackClients.Rtm.Connections.Clients.Handshake;
using Slackbot.Net.SlackClients.Rtm.Connections.Monitoring;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound;
using Slackbot.Net.SlackClients.Rtm.Exceptions;

namespace Slackbot.Net.SlackClients.Rtm
{
    public class Connector : IConnector
    {
        private readonly ILogger<Connector> _logger;

        private readonly IHandshakeClient _handshakeClient;
        private readonly IWebSocketClient _webSocket;
        private readonly IPingPongMonitor _pingPongMonitor;
        private readonly string _slackKey;

        public Connector(IOptions<RtmOptions> options, ILogger<Connector> logger = null) : this(options.Value, logger)
        {
        }
        
        public Connector(RtmOptions options, ILogger<Connector> logger = null) :
            this(new HandshakeClient(new HttpClient()),
                new WebSocketClientLite(new MessageInterpreter()),
                new PingPongMonitor(new Timer(), new DateTimeKeeper()),
                options.Token)
        {
            _logger = logger;
        }

        internal Connector(
            IHandshakeClient handshakeClient, 
            IWebSocketClient webSocket,
            IPingPongMonitor pingPongMonitor,
            string slackKey)
        {
            _handshakeClient = handshakeClient;
            _webSocket = webSocket;
            _pingPongMonitor = pingPongMonitor;
            _slackKey = slackKey;
            
            if (_logger == null)
                _logger = new NoOpLogger();
        }

        public async Task<IConnection> Connect()
        {
            if (string.IsNullOrEmpty(_slackKey))
            {
                throw new ArgumentNullException(nameof(_slackKey));
            }

            var handshakeResponse = await _handshakeClient.FirmShake(_slackKey);

            if (!handshakeResponse.Ok)
            {
                if (handshakeResponse.Error == "token_revoked" || handshakeResponse.Error == "account_inactive")
                {
                    throw new TokenRevokedException(handshakeResponse.Error);
                }

                if (handshakeResponse.Error == "missing_scope")
                {
                    throw new MissingScopeException(handshakeResponse.Error);

                }
                throw new HandshakeException(handshakeResponse.Error);
            }

            await _webSocket.Connect(handshakeResponse.WebSocketUrl);
            
            var connectionInfo = ConnectionInformationMapper.CreateConnectionInformation(_slackKey, handshakeResponse);

            var connection =  new Connection(_pingPongMonitor, _handshakeClient, _webSocket);
            await connection.Initialise(connectionInfo);
            
            return connection;
        }

      
    }

    public class NoOpLogger : ILogger<Connector>, IDisposable
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
            
        }
    }
}