using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Connections.Clients.Handshake;
using Slackbot.Net.SlackClients.Rtm.Connections.Responses;
using Slackbot.Net.SlackClients.Rtm.Tests.Integration.Configuration;
using Xunit;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Integration.Connections.Clients
{
    public class HandshakeClientTests
    {
        [Fact]
        public async Task should_perform_handshake()
        {
            // given
            var config = new ConfigReader().GetConfig();
            var client = new HandshakeClient(new HttpClient());

            // when
            var response = await client.FirmShake(config.Slack.ApiToken);

            // then
            response.ShouldNotBeNull();
            response.WebSocketUrl.ShouldNotBeEmpty();
        }
    }
}