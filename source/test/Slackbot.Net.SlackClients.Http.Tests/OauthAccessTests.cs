using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Http.Exceptions;
using Slackbot.Net.SlackClients.Http.Models.Requests.OAuthAccess;
using Xunit;
using Xunit.Abstractions;

namespace Slackbot.Net.Tests
{
    public class OauthAccessTests : Setup
    {
        public OauthAccessTests(ITestOutputHelper helper) : base(helper)
        {
        }
        
        [Fact]
        public async Task OauthAccessTestsThrowsInvalidCodeOnInvalidCodes()
        {
            var oauthAccessRequest = new OauthAccessRequest
            {
                ClientId = "lol",
                ClientSecret = "troll",
                Code = "jimbu"
            };
            var ex = await Assert.ThrowsAsync<SlackApiException>(() => SlackClient.OauthAccess(oauthAccessRequest));
            Assert.Equal("invalid_code", ex.Message);
        }
    }
}