using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests;

public class UsersProfileGetTests(ITestOutputHelper helper) : Setup(helper)
{
    [Fact]
    public async Task UsersProfileWorks()
    {
        var response = await SlackClient.UserProfile("USRAR1YTV");
        Assert.True(response.Ok);
    }

    [Fact]
    public async Task FindsApp()
    {
        var response = await SlackClient.UserProfile("USRAR1YTV");
        Assert.Equal("AREFP62B1", response.Profile.Api_App_Id);
    }
}
