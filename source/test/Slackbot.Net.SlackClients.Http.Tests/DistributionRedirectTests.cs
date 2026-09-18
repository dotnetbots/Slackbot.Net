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
    public async Task WithoutState_TheWorkspaceIsInstalledAndTheSuccessPageUsedUntouched()
    {
        var (ctx, handler) = await Install(state: null, successRedirectUri: "/success?default=1");

        var workspace = Assert.Single(handler.Installed);
        Assert.Equal("T1", workspace.TeamId);
        Assert.Equal("Team", workspace.TeamName);
        Assert.Equal("xoxb-token", workspace.Token);
        Assert.Equal("/success?default=1", ctx.Response.Headers.Location.ToString());
    }

    [Fact]
    public async Task StateRidesBackToTheSuccessPageAsAQueryParameter()
    {
        var (ctx, _) = await Install("/admin/slack", "/success?default=1");

        Assert.Equal("/success?default=1&state=%2Fadmin%2Fslack", ctx.Response.Headers.Location.ToString());
    }

    [Theory]
    [InlineData("/admin/slack")]
    [InlineData("https://evil.example/steal")]
    [InlineData("//evil.example")]
    [InlineData("eyJyZXR1cm5UbyI6Ii9hZG1pbiJ9")]
    public async Task StateIsNeverTheRedirectTarget_WhateverItHolds(string state)
    {
        var (ctx, _) = await Install(state, "https://example.com/success");

        Assert.StartsWith("https://example.com/success?state=", ctx.Response.Headers.Location.ToString());
    }

    private static async Task<(HttpContext Context, RecordingInstallationHandler Handler)> Install(
        string state, string successRedirectUri)
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

        public Task Uninstall(string teamId) => Task.CompletedTask;
    }
}
