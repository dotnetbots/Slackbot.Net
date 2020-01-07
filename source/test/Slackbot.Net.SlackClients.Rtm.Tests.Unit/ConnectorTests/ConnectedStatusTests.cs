using System;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Connections.Clients.Handshake;
using Slackbot.Net.SlackClients.Rtm.Connections.Monitoring;
using Slackbot.Net.SlackClients.Rtm.Connections.Responses;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets;
using Slackbot.Net.SlackClients.Rtm.Exceptions;
using Xunit;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.SlackConnectorTests
{
    public class ConnectedStatusTests
    {
        private string _slackKey = "slacKing-off-ey?";
        private readonly Mock<IHandshakeClient> _handshakeClient;
        private readonly Mock<IWebSocketClient> _webSocketClient;
        private readonly Mock<IPingPongMonitor> _pingPong;

        public ConnectedStatusTests()
        {
            _handshakeClient = new Mock<IHandshakeClient>();
            _webSocketClient = new Mock<IWebSocketClient>();
            _pingPong = new Mock<IPingPongMonitor>();
            
        }

        [Fact]
        public async Task should_throw_exception_when_handshake_is_not_ok()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = false,
                Error = "I AM A ERROR"
            };

            _handshakeClient
                .Setup(x => x.FirmShake(_slackKey))
                .ReturnsAsync(handshakeResponse);
            
            var slackConnector = new Connector(_handshakeClient.Object, _webSocketClient.Object, _pingPong.Object, _slackKey);

            // when
            var exception = await Assert.ThrowsAsync<HandshakeException>(() => slackConnector.Connect());

            // then
            exception.Message.ShouldBe(handshakeResponse.Error);
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