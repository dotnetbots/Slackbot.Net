using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Rtm.Connections.Clients.Handshake;
using Slackbot.Net.SlackClients.Rtm.Connections.Monitoring;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Outbound;
using Slackbot.Net.SlackClients.Rtm.EventHandlers;
using Slackbot.Net.SlackClients.Rtm.Extensions;
using Slackbot.Net.SlackClients.Rtm.Models;

namespace Slackbot.Net.SlackClients.Rtm
{
    internal class Connection : IConnection
    {
        private readonly IWebSocketClient _webSocketClient;
        private readonly IPingPongMonitor _pingPongMonitor;
        private readonly IHandshakeClient _handshakeClient;

        public bool IsConnected => _webSocketClient?.IsAlive ?? false;

        public ContactDetails Team { get; private set; }
        public ContactDetails Self { get; private set; }
        
        public IReadOnlyDictionary<string, ChatHub> ConnectedHubs { get; private set; }
        public DateTime? ConnectedSince { get; private set; }

        public string SlackKey { get; private set; }

        public Connection(IPingPongMonitor pingPongMonitor, 
            IHandshakeClient handshakeClient,
            IWebSocketClient webSocketClient)
        {
            _webSocketClient = webSocketClient;
            _pingPongMonitor = pingPongMonitor;
            _handshakeClient = handshakeClient;
        }

        internal async Task Initialise(ConnectionInformation connectionInformation)
        {
            SlackKey = connectionInformation.SlackKey;
            Team = connectionInformation.Team;
            Self = connectionInformation.Self;
            _userCache = connectionInformation.Users;
            ConnectedHubs = connectionInformation.SlackChatHubs;

            _webSocketClient.OnClose += (sender, args) =>
            {
                ConnectedSince = null;
                RaiseOnDisconnect();
            };
            ConnectedSince = DateTime.Now;

            _webSocketClient.OnMessage += async (sender, message) => await ListenTo(message);

            await _pingPongMonitor.StartMonitor(Ping, Reconnect, TimeSpan.FromMinutes(2));
        }

        private async Task Reconnect()
        {
            var reconnectingEvent = RaiseOnReconnecting();

            var handshake = await _handshakeClient.FirmShake(SlackKey);
            await _webSocketClient.Connect(handshake.WebSocketUrl);

            await Task.WhenAll(reconnectingEvent, RaiseOnReconnect());
        }

        private Task ListenTo(InboundMessage inboundMessage)
        {
            if (inboundMessage == null)
            {
                return Task.CompletedTask;
            }

            switch (inboundMessage.MessageType)
            {
                case MessageType.Message: return HandleMessage((ChatMessage)inboundMessage);
                case MessageType.Pong: return HandlePong((PongMessage)inboundMessage);
            }

            return Task.CompletedTask;
        }

        private Task HandleMessage(ChatMessage inboundMessage)
        {
            if (string.IsNullOrEmpty(inboundMessage.User))
                return Task.CompletedTask;

            if (!string.IsNullOrEmpty(Self.Id) && inboundMessage.User == Self.Id)
                return Task.CompletedTask;

            var message = new Message
            {
                User = GetMessageUser(inboundMessage.User),
                Timestamp = inboundMessage.Timestamp,
                Text = inboundMessage.Text,
                ChatHub = GetChatHub(inboundMessage.Channel),
                RawData = inboundMessage.RawData,
                MentionsBot = WasBotMentioned(Self.Name, Self.Id, inboundMessage.Text),
                MessageSubType = inboundMessage.MessageSubType.ToSlackMessageSubType(),
                Files = inboundMessage.Files.ToSlackFiles()
            };

            return RaiseMessageReceived(message);
        }
        
        private ChatHub GetChatHub(string channel)
        {
            return channel != null && ConnectedHubs.ContainsKey(channel)
                ? ConnectedHubs[channel]
                : null;
        }

        private Task HandlePong(PongMessage inboundMessage)
        {
            _pingPongMonitor.Pong();
            return RaisePong(inboundMessage.Timestamp);
        }

        private Dictionary<string, User> _userCache { get; set; }
        public IReadOnlyDictionary<string, User> UserCache => _userCache;

        
        private User GetMessageUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            return UserCache.ContainsKey(userId)
                ? UserCache[userId]
                : new User { Id = userId, Name = string.Empty };
        }

        public async Task Close()
        {
            if (_webSocketClient != null && _webSocketClient.IsAlive)
            {
                await _webSocketClient.Close();
            }
        }
        public event DisconnectEventHandler OnDisconnect;
        private void RaiseOnDisconnect()
        {
            OnDisconnect?.Invoke();
        }

        public event ReconnectEventHandler OnReconnecting;
        private async Task RaiseOnReconnecting()
        {
            var e = OnReconnecting;
            if (e != null)
            {
                try
                {
                    await e();
                }
                catch (Exception)
                {

                }
            }
        }

        public event ReconnectEventHandler OnReconnect;
        private async Task RaiseOnReconnect()
        {
            var e = OnReconnect;
            if (e != null)
            {
                try
                {
                    await e();
                }
                catch (Exception)
                {

                }
            }
        }

        public event MessageReceivedEventHandler OnMessageReceived;
        private async Task RaiseMessageReceived(Message message)
        {
            var e = OnMessageReceived;
            if (e != null)
            {
                try
                {
                    await e(message);
                }
                catch (Exception)
                {

                }
            }
        }

        public async Task Ping()
        {
            await _webSocketClient.SendMessage(new PingMessage());
        }
        
        public event PongEventHandler OnPong;
        private async Task RaisePong(DateTime timestamp)
        {
            var e = OnPong;
            if (e != null)
            {
                try
                {
                    await e(timestamp);
                }
                catch
                {
                }
            }
        }
        
        public bool WasBotMentioned(string username, string userId, string messageText)
        {
            bool mentioned = false;

            if (!string.IsNullOrEmpty(messageText) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(userId))
            {
                string regexText = $"<@{userId}>|{username}";
                mentioned = Regex.IsMatch(messageText, regexText, RegexOptions.IgnoreCase);
            }

            return mentioned;
        }
    }
}
