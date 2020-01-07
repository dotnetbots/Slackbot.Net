using System;
using System.Net.Http;
using System.Threading.Tasks;
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

        private readonly IHandshakeClient _handshakeClient;
        private readonly IWebSocketClient _webSocket;
        private readonly IPingPongMonitor _pingPongMonitor;
        private readonly string _slackKey;

        public Connector(IOptions<RtmOptions> options) : this(options.Value)
        {
        }
        
        public Connector(RtmOptions options) :
            this(new HandshakeClient(new HttpClient()),
                new WebSocketClientLite(new MessageInterpreter()),
                new PingPongMonitor(new Timer(), new DateTimeKeeper()),
                options.ApiKey)
        {
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
                throw new HandshakeException(handshakeResponse.Error);
            }

            await _webSocket.Connect(handshakeResponse.WebSocketUrl);
            
            var connectionInfo = ConnectionInformationMapper.CreateConnectionInformation(_slackKey, handshakeResponse);

            var connection =  new Connection(_pingPongMonitor, _handshakeClient, _webSocket);
            await connection.Initialise(connectionInfo);
            
            return connection;
        }

      
    }
}