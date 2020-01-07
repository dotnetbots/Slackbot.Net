using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Moq;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets;
using Slackbot.Net.SlackClients.Rtm.Models;
using Xunit;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.SlackConnectionTests
{
    public class CloseConnectionTests
    {
        [Theory, AutoMoqData]
        private async Task should_close_websocket_when_websocket_is_connected(
            [Frozen]Mock<IWebSocketClient> webSocket, 
            Connection connection)
        {
            // given
            webSocket
                .Setup(x => x.IsAlive)
                .Returns(true);

            var info = GetDummyConnectionInformation(webSocket);
            await connection.Initialise(info);

            // when
            await connection.Close();

            // then
            webSocket.Verify(x => x.Close(), Times.Once);
        }

        [Theory, AutoMoqData]
        private async Task should_not_close_websocket_when_websocket_is_disconnected(
            [Frozen]Mock<IWebSocketClient> webSocket, 
            Connection connection)
        {
            // given
            webSocket
                .Setup(x => x.IsAlive)
                .Returns(false);

            var info = GetDummyConnectionInformation(webSocket);
            await connection.Initialise(info);

            // when
            await connection.Close();

            // then
            webSocket.Verify(x => x.Close(), Times.Never);
        }

        private static ConnectionInformation GetDummyConnectionInformation(Mock<IWebSocketClient> webSocket)
        {
            var info = new ConnectionInformation
            {
                Self = new ContactDetails { Id = "self-id" },
                Team = new ContactDetails { Id = "team-id" },
                Users = new Dictionary<string, User> { { "userid", new User() { Name = "userName" } } },
                SlackChatHubs = new Dictionary<string, ChatHub> { { "some-hub", new ChatHub() } },
            };
            return info;
        }
    }
}