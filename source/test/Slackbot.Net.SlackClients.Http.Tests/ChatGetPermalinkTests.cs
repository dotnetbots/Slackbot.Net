using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Slackbot.Net.Tests
{
    public class ChatGetPermalinkTests : Setup
    {
        public ChatGetPermalinkTests(ITestOutputHelper helper) : base(helper)
        {
        }
        
        [Fact]
        public async Task GetPermalinkWorks()
        {
            var response = await SlackClient.ChatPostMessage(Channel, Text);
            var permalink = await SlackClient.ChatGetPermalink(response.channel, response.ts);
            Assert.True(permalink.Ok);
            Assert.NotNull(permalink.Permalink);
        }
    }
}