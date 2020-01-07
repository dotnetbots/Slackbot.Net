using System;
using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Outbound;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Sockets
{
    internal interface IWebSocketClient
    {
        bool IsAlive { get; }

        Task Connect(string webSockerUrl);
        Task SendMessage(BaseMessage message);
        Task Close();

        event EventHandler<InboundMessage> OnMessage;
        event EventHandler OnClose;
    }
}