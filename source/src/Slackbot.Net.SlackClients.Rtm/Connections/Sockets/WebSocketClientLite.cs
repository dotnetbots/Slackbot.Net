using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Outbound;
using Slackbot.Net.SlackClients.Rtm.Exceptions;
using Websocket.Client;
using Websocket.Client.Models;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Sockets
{
   internal class WebSocketClient : IWebSocketClient
    {
        private readonly IMessageInterpreter _interpreter;
        private WebsocketClient _client;
        private readonly TimeSpan _reconnectTimeout = TimeSpan.FromSeconds(30);
        private int _currentMessageId;

        public WebSocketClient(IMessageInterpreter interpreter)
        {
            _interpreter = interpreter;
        }

        public bool IsAlive => _client.IsRunning;

        public async Task Connect(string webSocketUrl)
        {
            if (_client != null)
            {
                Console.WriteLine($"Connect called, closing current connection. IsAlive {IsAlive}");
                await Close();
            }

            var uri = new Uri(webSocketUrl);
            _client = new WebsocketClient(uri)
            {
                ReconnectTimeout = _reconnectTimeout,
                IsReconnectionEnabled = false
            };
            _client.MessageReceived.Subscribe(MessageReceived);
            _client.DisconnectionHappened.Subscribe(Disconnected);
            _client.ReconnectionHappened.Subscribe(Reconnected);

            await _client.Start();
        }

        public void Reconnected(ReconnectionInfo info)
        {
            Console.WriteLine($"Reconnected {info.Type}");
        }

        public Task SendMessage(BaseMessage message)
        {
            if (!IsAlive)
            {
                throw new CommunicationException("Connection not Alive");
            }

            System.Threading.Interlocked.Increment(ref _currentMessageId);
            message.Id = _currentMessageId;
            var json = JsonConvert.SerializeObject(message);

            _client.Send(json);

            return Task.CompletedTask;
        }

        public Task Close()
        {
            _client.Dispose();
            return Task.CompletedTask;
        }

        public event EventHandler OnClose;
        private void Disconnected(DisconnectionInfo obj)
        {
            Console.WriteLine($"Reconnected {obj.Type}");
            OnClose?.Invoke(this, null);
        }

        public event EventHandler<InboundMessage> OnMessage;
        private void MessageReceived(ResponseMessage message)
        {
            string messageJson = message.Text ?? "";
            var inboundMessage = _interpreter.InterpretMessage(messageJson);
            OnMessage?.Invoke(this, inboundMessage);
        }
    }
}