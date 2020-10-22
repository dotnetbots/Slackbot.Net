using System;
using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Http.Exceptions;
using Slackbot.Net.SlackClients.Http.Models.Requests.OAuthAccess;
using Xunit;
using Xunit.Abstractions;

namespace CronBackgroundServices.Tests
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
                ClientId = Environment.GetEnvironmentVariable("CLIENT_ID"),
                ClientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET"),
                Code = ""
            };
            var ex = Assert.Throws<WellKnownSlackApiException>(() =>
            {
                var oAuthAccess = SlackOAuthClient.OAuthAccess(oauthAccessRequest).GetAwaiter().GetResult();
                return oAuthAccess;
            });
            Assert.Equal("invalid_code", ex.Message);
        }
    }
}