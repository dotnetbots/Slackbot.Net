using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Slackbot.Net.Tests
{
    public class AddReactionTests : Setup
    {
        public AddReactionTests(ITestOutputHelper helper) : base(helper)
        {
        }
        
        [Fact]
        public async Task AddReactionWorks()
        {
            var response = await SlackClient.ChatPostMessage(Channel, Text);
            var reactionResponse = await SlackClient.ReactionsAdd("thumbsup", response.channel, response.ts);
            Assert.True(reactionResponse.Ok);
        }
    }
}