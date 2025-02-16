using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests;

public class SearchTests(ITestOutputHelper helper) : Setup(helper)
{
    [Fact]
    public async Task SearchForLinksWorks()
    {
        var response = await SearchClient.SearchMessagesAsync("http");
        Assert.True(response.Ok);
        Assert.NotEmpty(response.Messages.Matches);
    }
}
