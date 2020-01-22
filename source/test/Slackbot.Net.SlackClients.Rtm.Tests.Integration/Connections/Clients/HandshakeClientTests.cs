using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Connections.Clients.Handshake;
using Xunit;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Integration.Connections.Clients
{
    public class HandshakeClientTests : IntegrationTest
    {
        [Fact]
        public async Task should_perform_handshake()
        {
            // given
            var client = new HandshakeClient(new HttpClient());

            // when
            var response = await client.FirmShake(Token);

            // then
            response.ShouldNotBeNull();
            response.WebSocketUrl.ShouldNotBeEmpty();
        }
    }
}