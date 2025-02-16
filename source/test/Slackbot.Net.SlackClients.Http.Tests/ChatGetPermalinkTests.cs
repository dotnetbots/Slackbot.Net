using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests;

public class ChatGetPermalinkTests(ITestOutputHelper helper) : Setup(helper)
{
    [Fact]
    public async Task GetPermalinkWorks()
    {
        var response = await SlackClient.ChatPostMessage(Channel, Text);
        var permalink = await SlackClient.ChatGetPermalink(response.channel, response.ts);
        Assert.True(permalink.Ok);
        Assert.NotNull(permalink.Permalink);
    }
}
