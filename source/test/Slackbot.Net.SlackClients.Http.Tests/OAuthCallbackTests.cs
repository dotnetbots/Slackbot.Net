using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Endpoints.Hosting;

namespace Slackbot.Net.Tests;

public class OAuthCallbackTests
{
    private const string Installed =
        """{"ok":true,"access_token":"xoxb-token","scope":"chat:write","team":{"id":"T1","name":"Team"},"app_id":"A1"}""";

    [Theory]
    [InlineData("/slack/oauth", "", "https://example.com/slack/oauth")]
    [InlineData("", "/slack/oauth", "https://example.com/slack/oauth")]
    [InlineData("/app", "/slack/oauth", "https://example.com/app/slack/oauth")]
    public async Task TheRedirectUriIsTheCallbacksUrl_MountedWithMapOrAsAnEndpoint(string pathBase, string path, string redirectUri)
    {
        var callback = await CallbackAsync("?code=the-code", pathBase: pathBase, path: path);

        Assert.Equal(redirectUri, Assert.Single(callback.Exchanges)["redirect_uri"]);
    }

    [Fact]
    public async Task ADeclinedInstall_GoesToTheCancelledPageWithItsState_AndExchangesNothing()
    {
        var callback = await CallbackAsync("?error=access_denied&state=s1", o => o.CancelledRedirectUri = "/cancelled");

        Assert.Equal("/cancelled?state=s1", callback.Context.Response.Headers.Location.ToString());
        Assert.Empty(callback.Exchanges);
        Assert.Empty(callback.Handler.Installed);
    }

    [Fact]
    public async Task ACallbackWithoutACode_GoesOnToTheNextMiddleware_AndExchangesNothing()
    {
        var callback = await CallbackAsync("?error=access_denied");

        Assert.True(callback.ReachedNext);
        Assert.Empty(callback.Exchanges);
        Assert.Empty(callback.Handler.Installed);
    }

    [Fact]
    public async Task AStateTheAppRefuses_ExchangesNothing_AndInstallsNothing()
    {
        var callback = await CallbackAsync("?code=the-code&state=forged", o =>
        {
            o.ValidateState = (_, state) => Task.FromResult(state == "the-one-we-sent");
            o.FailedRedirectUri = "/failed";
        });

        Assert.Equal("/failed", callback.Context.Response.Headers.Location.ToString());
        Assert.Empty(callback.Exchanges);
        Assert.Empty(callback.Handler.Installed);
    }

    [Fact]
    public async Task AStateTheAppAccepts_Installs()
    {
        HttpContext seen = null;
        var callback = await CallbackAsync("?code=the-code&state=the-one-we-sent", o => o.ValidateState = (ctx, state) =>
        {
            seen = ctx;
            return Task.FromResult(state == "the-one-we-sent");
        });

        Assert.Same(callback.Context, seen);
        Assert.Single(callback.Handler.Installed);
    }

    [Fact]
    public async Task ARefusedStateWithoutAFailedPage_IsABare403()
    {
        var callback = await CallbackAsync("?code=the-code&state=forged", o => o.ValidateState = (_, _) => Task.FromResult(false));

        Assert.Equal(StatusCodes.Status403Forbidden, callback.Context.Response.StatusCode);
        Assert.Empty(callback.Exchanges);
    }

    [Fact]
    public async Task ACodeSlackRefuses_GoesToTheFailedPage_AndInstallsNothing()
    {
        var callback = await CallbackAsync(
            "?code=stale", o => o.FailedRedirectUri = "/failed", oauthAccessResponse: """{"ok":false,"error":"invalid_code"}""");

        Assert.Equal("/failed", callback.Context.Response.Headers.Location.ToString());
        Assert.Empty(callback.Handler.Installed);
    }

    private static async Task<Callback> CallbackAsync(
        string query, Action<OAuthOptions> configure = null, string pathBase = "/oauth", string path = "",
        string oauthAccessResponse = Installed)
    {
        var slack = new SlackApiStub(oauthAccessResponse);
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSlackBotEvents<RecordingInstallationHandler>();
        services.AddSlackbotDistribution(o =>
        {
            o.CLIENT_ID = "client-id";
            o.CLIENT_SECRET = "client-secret";
            o.SuccessRedirectUri = "/success";
            configure?.Invoke(o);
        });
        services.ConfigureHttpClientDefaults(b => b.ConfigurePrimaryHttpMessageHandler(() => slack));

        await using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        await using var scope = provider.CreateAsyncScope();

        var reachedNext = false;
        var pipeline = new ApplicationBuilder(provider)
            .UseSlackbotDistribution()
            .Use(_ => _ =>
            {
                reachedNext = true;
                return Task.CompletedTask;
            })
            .Build();

        var ctx = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
        ctx.Request.Scheme = "https";
        ctx.Request.Host = new HostString("example.com");
        ctx.Request.PathBase = pathBase;
        ctx.Request.Path = path;
        ctx.Request.QueryString = new QueryString(query);

        await pipeline(ctx);

        var handler = (RecordingInstallationHandler)scope.ServiceProvider.GetRequiredService<IWorkspaceInstallationHandler>();
        return new Callback(ctx, handler, slack.Exchanges, reachedNext);
    }

    private sealed record Callback(HttpContext Context, RecordingInstallationHandler Handler, List<Dictionary<string, string>> Exchanges, bool ReachedNext);

    private sealed class SlackApiStub(string oauthAccessResponse) : HttpMessageHandler
    {
        public List<Dictionary<string, string>> Exchanges { get; } = [];

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri!.AbsolutePath.EndsWith("oauth.v2.access"))
            {
                var form = await request.Content!.ReadAsStringAsync(cancellationToken);
                Exchanges.Add(QueryHelpers.ParseQuery(form).ToDictionary(p => p.Key, p => p.Value.ToString()));
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(oauthAccessResponse, System.Text.Encoding.UTF8, "application/json")
            };
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
