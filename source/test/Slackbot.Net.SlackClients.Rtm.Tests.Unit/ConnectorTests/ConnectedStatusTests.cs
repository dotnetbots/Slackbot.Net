using System;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Connections.Clients.Handshake;
using Slackbot.Net.SlackClients.Rtm.Connections.Monitoring;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets;
using Xunit;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.SlackConnectorTests
{
    public class ConnectedStatusTests
    {
        private readonly Mock<IHandshakeClient> _handshakeClient;
        private readonly Mock<IWebSocketClient> _webSocketClient;
        private readonly Mock<IPingPongMonitor> _pingPong;

        public ConnectedStatusTests()
        {
            _handshakeClient = new Mock<IHandshakeClient>();
            _webSocketClient = new Mock<IWebSocketClient>();
            _pingPong = new Mock<IPingPongMonitor>();
            
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task should_throw_exception_given_empty_api_key(string slackKey)
        {
            // given
            var slackConnector = new Connector(_handshakeClient.Object, _webSocketClient.Object, _pingPong.Object, slackKey);

            // when
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnector.Connect());

            // then
            exception.Message.ShouldContain("slackKey");
        }
    }
}