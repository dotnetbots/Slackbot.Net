using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests;

public class AddReactionTests(ITestOutputHelper helper) : Setup(helper)
{
    [Fact]
    public async Task AddReactionWorks()
    {
        var response = await SlackClient.ChatPostMessage(Channel, Text);
        var reactionResponse = await SlackClient.ReactionsAdd("thumbsup", response.channel, response.ts);
        Assert.True(reactionResponse.Ok);
    }
}
