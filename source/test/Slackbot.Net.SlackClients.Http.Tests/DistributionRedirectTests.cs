using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Endpoints.Hosting;
using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests;

public class DistributionRedirectTests
{
    private const string OauthAccessResponse =
        """{"ok":true,"access_token":"xoxb-token","scope":"chat:write","team":{"id":"T1","name":"Team"},"app_id":"A1"}""";

    [Fact]
    public async Task InstallsWorkspaceAndRedirectsToSuccessRedirectUri()
    {
        var (ctx, handler) = await Install(null, "/success?default=1");

        var workspace = Assert.Single(handler.Installed);
        Assert.Equal("T1", workspace.TeamId);
        Assert.Equal("Team", workspace.TeamName);
        Assert.Equal("xoxb-token", workspace.Token);
        Assert.Equal(StatusCodes.Status302Found, ctx.Response.StatusCode);
        Assert.Equal("/success?default=1", ctx.Response.Headers.Location.ToString());
    }

    [Theory]
    // A site-relative state keeps the origin of an absolute SuccessRedirectUri
    [InlineData("/admin/slack?installed=1", "https://example.com/success?default=1",
        "https://example.com/admin/slack?installed=1")]
    [InlineData("/", "https://example.com/success", "https://example.com/")]
    // ... and is used as-is when SuccessRedirectUri is relative too
    [InlineData("/admin/slack", "/success?default=1", "/admin/slack")]
    // Anything that could leave the site is ignored
    [InlineData("https://evil.example/steal", "https://example.com/success", "https://example.com/success")]
    [InlineData("//evil.example", "https://example.com/success", "https://example.com/success")]
    [InlineData("/\\evil.example", "https://example.com/success", "https://example.com/success")]
    [InlineData("admin/slack", "https://example.com/success", "https://example.com/success")]
    [InlineData("", "https://example.com/success", "https://example.com/success")]
    public async Task RedirectsToStateWhenItIsSiteRelative(string state, string successRedirectUri,
        string expectedLocation)
    {
        var (ctx, _) = await Install(state, successRedirectUri);

        Assert.Equal(expectedLocation, ctx.Response.Headers.Location.ToString());
    }

    private static async Task<(HttpContext Context, RecordingInstallationHandler Handler)> Install(string state,
        string successRedirectUri)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSlackBotEvents<RecordingInstallationHandler>();
        services.AddSlackbotDistribution(o =>
        {
            o.CLIENT_ID = "client-id";
            o.CLIENT_SECRET = "client-secret";
            o.SuccessRedirectUri = successRedirectUri;
        });
        // Serves a canned oauth.v2.access response, so no call goes to Slack
        services.ConfigureHttpClientDefaults(b =>
            b.ConfigurePrimaryHttpMessageHandler(() => new StubHttpMessageHandler(OauthAccessResponse)));

        await using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        await using var scope = provider.CreateAsyncScope();

        var pipeline = new ApplicationBuilder(provider).UseSlackbotDistribution().Build();

        var query = QueryString.Create("code", "the-code");
        if (state != null)
        {
            query = query.Add(QueryString.Create("state", state));
        }

        var ctx = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
        ctx.Request.Scheme = "https";
        ctx.Request.Host = new HostString("example.com");
        ctx.Request.QueryString = query;

        await pipeline(ctx);

        var handler = (RecordingInstallationHandler)scope.ServiceProvider
            .GetRequiredService<IWorkspaceInstallationHandler>();
        return (ctx, handler);
    }

    private sealed class RecordingInstallationHandler : IWorkspaceInstallationHandler
    {
        public List<Workspace> Installed { get; } = [];

        public Task Install(Workspace workspace)
        {
            Installed.Add(workspace);
            return Task.CompletedTask;
        }

        public Task Uninstall(string teamId)
        {
            return Task.CompletedTask;
        }
    }
}
