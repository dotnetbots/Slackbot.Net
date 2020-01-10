using System;
using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Http;
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
        public void OauthAccessTestsThrowsInvalidCodeOnInvalidCodes()
        {
            var oauthAccessRequest = new OauthAccessRequest
            {
                ClientId = "lol",
                ClientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET"),
                Code = "troll"
            };
            var ex = Assert.Throws<SlackApiException>(() =>
            {
                var oAuthAccess = SlackOAuthClient.OAuthAccess(oauthAccessRequest).GetAwaiter().GetResult();
                return oAuthAccess;
            });
            Assert.Equal("invalid_code", ex.Message);
        }
    }
}