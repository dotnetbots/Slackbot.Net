using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Http.Exceptions;
using Slackbot.Net.SlackClients.Http.Models.Requests.ChatPostMessage;
using Xunit;
using Xunit.Abstractions;

namespace Slackbot.Net.Tests
{
    public class ChatPostMessageTests : Setup
    {

        public ChatPostMessageTests(ITestOutputHelper helper) : base(helper)
        {
            
        }
        
        [Fact]
        public async Task PostMinimalWorks()
        {
            var response = await SlackClient.ChatPostMessage(Channel, Text);
            Assert.True(response.Ok);
        }
        
        [Fact]
        public async Task PostWorks()
        {
            var msg = new ChatPostMessageRequest
            {
                Channel = Channel,
                Text = Text
            };
            var response = await SlackClient.ChatPostMessage(msg);
            Assert.True(response.Ok);
        }
        
        [Fact]
        public async Task PostMissingChannelThrowsSlackApiException()
        {
            await Assert.ThrowsAsync<SlackApiException>(() => SlackClient.ChatPostMessage("", Text));
        }
    }
}