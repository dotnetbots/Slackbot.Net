using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Endpoints.Hosting;

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
    public async Task TheInstallingUserIsCarriedToTheInstallationHandler()
    {
        var (_, handler) = await Install(state: null, successRedirectUri: "/success",
            """{"ok":true,"access_token":"xoxb-token","scope":"chat:write","team":{"id":"T1","name":"Team"},"app_id":"A1","authed_user":{"id":"U1"}}""");

        Assert.Equal("U1", Assert.Single(handler.Installed).InstallerUserId);
    }

    [Fact]
    public async Task WithoutAnAuthedUser_TheWorkspaceIsInstalledWithNoInstaller()
    {
        var (_, handler) = await Install(state: null, successRedirectUri: "/success");

        Assert.Null(Assert.Single(handler.Installed).InstallerUserId);
    }

    [Fact]
    public async Task AnInstallerGrantingOpenId_IsIdentifiedByTheirSlackProfile()
    {
        var (_, handler) = await Install(state: null, successRedirectUri: "/success",
            OauthAccessResponseWithUserScope("openid,email,profile"),
            """{"ok":true,"sub":"U1","email":"installer@example.com","email_verified":true,"name":"Ina Installer"}""");

        Assert.Equal(new InstallerIdentity("installer@example.com", true, "Ina Installer"), Assert.Single(handler.Installed).Installer);
    }

    [Fact]
    public async Task AnInstallerNotGrantingOpenId_HasNoIdentity()
    {
        var (_, handler) = await Install(state: null, successRedirectUri: "/success",
            OauthAccessResponseWithUserScope("search:read"),
            """{"ok":true,"sub":"U1","email":"installer@example.com","email_verified":true,"name":"Ina Installer"}""");

        Assert.Null(Assert.Single(handler.Installed).Installer);
    }

    [Fact]
    public async Task AFailedProfileLookup_StillInstallsTheWorkspace()
    {
        var (_, handler) = await Install(state: null, successRedirectUri: "/success",
            OauthAccessResponseWithUserScope("openid,email,profile"),
            """{"ok":false,"error":"invalid_auth"}""");

        var workspace = Assert.Single(handler.Installed);
        Assert.Equal("U1", workspace.InstallerUserId);
        Assert.Null(workspace.Installer);
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
        string state, string successRedirectUri, string oauthAccessResponse = OauthAccessResponse,
        string userInfoResponse = null)
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
            b.ConfigurePrimaryHttpMessageHandler(() => new SlackApiStub(oauthAccessResponse, userInfoResponse)));

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

    private static string OauthAccessResponseWithUserScope(string userScope) =>
        $$$"""{"ok":true,"access_token":"xoxb-token","scope":"chat:write","team":{"id":"T1","name":"Team"},"app_id":"A1","authed_user":{"id":"U1","scope":"{{{userScope}}}","access_token":"xoxp-token","token_type":"user"}}""";

    private sealed class SlackApiStub(string oauthAccessResponse, string userInfoResponse) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var json = request.RequestUri!.AbsolutePath.EndsWith("openid.connect.userInfo")
                ? userInfoResponse ?? """{"ok":false,"error":"not_stubbed"}"""
                : oauthAccessResponse;
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            });
        }
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
