using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IWebsocketClientLite.PCL;
using Newtonsoft.Json;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Outbound;
using WebsocketClientLite.PCL;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Sockets
{
    internal class WebSocketClientLite : IWebSocketClient
    {
        private readonly IMessageInterpreter _interpreter;
        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();
        private IMessageWebSocketRx _webSocket;
        private int _currentMessageId;
        private bool _isAlive;
        private Uri _uri;

        public bool IsAlive
        {
            get { return _isAlive; }
        }
        public WebSocketClientLite(IMessageInterpreter interpreter)
        {
            _interpreter = interpreter;
        }

        public async Task Connect(string webSockerUrl)
        {
            if (_webSocket != null)
            {
                await Close();
            }

            _webSocket = new MessageWebSocketRx
            {
                ExcludeZeroApplicationDataInPong = true
            };
            _uri = new Uri(webSockerUrl);
            _webSocket.ConnectionStatusObservable.Subscribe(OnConnectionChange);
            _webSocket.MessageReceiverObservable.Subscribe(OnWebSocketOnMessage);
            await _webSocket.ConnectAsync(_uri);
        }

        public async Task SendMessage(BaseMessage message)
        {
            System.Threading.Interlocked.Increment(ref _currentMessageId);
            message.Id = _currentMessageId;
            var json = JsonConvert.SerializeObject(message);

            if (_webSocket == null)
            {
                Console.WriteLine("WebSocket was null");
            }
            else
            {
                await _webSocket.SendTextAsync(json);
            }
        }

        public async Task Close()
        {
            using (_webSocket)
            {
                foreach (var subscription in _subscriptions)
                {
                    subscription.Dispose();
                }
                _subscriptions.Clear();

                await _webSocket.DisconnectAsync();
            }
        }

        public event EventHandler<InboundMessage> OnMessage;
        private void OnWebSocketOnMessage(string message)
        {
            string messageJson = message ?? "";
            var inboundMessage = _interpreter.InterpretMessage(messageJson);
            OnMessage?.Invoke(this, inboundMessage);
        }

        public event EventHandler OnClose;
        private void OnConnectionChange(ConnectionStatus connectionStatus)
        {
            System.Console.WriteLine(connectionStatus.ToString());

            switch (connectionStatus)
            {
                case ConnectionStatus.WebsocketConnected:
                    _isAlive = true;
                    break;
                case ConnectionStatus.Aborted:
                case ConnectionStatus.ConnectionFailed:
                case ConnectionStatus.Disconnected:
                    OnClose?.Invoke(this, null);
                    _isAlive = false;
                    break;
            }
        }
    }
}