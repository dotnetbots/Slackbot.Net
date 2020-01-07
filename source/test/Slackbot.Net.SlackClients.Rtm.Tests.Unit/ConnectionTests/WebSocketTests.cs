using System;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Connections.Clients.Handshake;
using Slackbot.Net.SlackClients.Rtm.Connections.Monitoring;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets;
using Slackbot.Net.SlackClients.Rtm.Models;
using Xunit;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.SlackConnectionTests
{
    public class WebSocketTests
    {
        [Theory, AutoMoqData]
        private async Task should_detect_disconnect(
            Mock<IWebSocketClient> webSocket,
            Mock<IPingPongMonitor> pingPongMonitor,
            Mock<IHandshakeClient> handShakeClient)
        {
            
            var slackConnection = new Connection(pingPongMonitor.Object, handShakeClient.Object, webSocket.Object);

            // given
            bool connectionChangedValue = false;
            slackConnection.OnDisconnect += () => connectionChangedValue = true;

            var info = new ConnectionInformation();
            await slackConnection.Initialise(info);

            // when
            webSocket.Raise(x => x.OnClose += null, new EventArgs());

            // then
            connectionChangedValue.ShouldBeTrue();
            slackConnection.IsConnected.ShouldBeFalse();
        }
    }
}