using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Endpoints.Hosting;
using Slackbot.Net.Endpoints.OAuth;

namespace Slackbot.Net.Endpoints.Middlewares;

internal class SlackbotCodeTokenExchangeMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext ctx, OAuthClient oAuthAccessClient,
        IOptions<OAuthOptions> options, IWorkspaceInstallationHandler installationHandler,
        ILogger<SlackbotCodeTokenExchangeMiddleware> logger)
    {
        var settings = options.Value;
        var code = ctx.Request.Query["code"].FirstOrDefault();
        var stateTheAppSent = ctx.Request.Query["state"].FirstOrDefault();

        if (string.IsNullOrEmpty(code))
        {
            var error = ctx.Request.Query["error"].FirstOrDefault();
            logger.LogInformation("Install callback without a code: {Error}", error);
            if (settings.CancelledRedirectUri is { } cancelled)
            {
                var page = SuccessRedirect(cancelled, stateTheAppSent);
                ctx.Response.Redirect(string.IsNullOrEmpty(error) ? page : QueryHelpers.AddQueryString(page, "error", error));
                return;
            }

            await next(ctx);
            return;
        }

        if (settings.ValidateState is { } validate && !await validate(ctx, stateTheAppSent))
        {
            logger.LogWarning("Install callback refused: the app didn't accept its state");
            await Fail(ctx, settings, StatusCodes.Status403Forbidden, null);
            return;
        }

        OAuthClient.OAuthAccessV2Response response;
        try
        {
            response = await oAuthAccessClient.OAuthAccessV2(new OAuthClient.OauthAccessV2Request(
                code,
                settings.CLIENT_ID,
                settings.CLIENT_SECRET,
                RedirectUri(ctx.Request)
            ));
        }
        catch (Exception e) when (settings.FailedRedirectUri is not null && e is HttpRequestException or SlackApiException or System.Text.Json.JsonException)
        {
            logger.LogError(e, "Slack refused to exchange the install's code");
            await Fail(ctx, settings, StatusCodes.Status500InternalServerError, null);
            return;
        }

        if (!response.Ok)
        {
            logger.LogError("Bad Oauth response! {Error}", response.Error);
            await Fail(ctx, settings, StatusCodes.Status500InternalServerError, response.Error);
            return;
        }

        logger.LogInformation("Oauth response! ok:{Ok}", response.Ok);
        await installationHandler.Install(new Workspace
        (
            response.Team.Id,
            response.Team.Name,
            response.Access_Token,
            await Installer(oAuthAccessClient, response.Authed_User, logger)
        ));

        ctx.Response.Redirect(SuccessRedirect(settings.SuccessRedirectUri, stateTheAppSent));
    }

    // Slack requires the exchange's redirect_uri to equal the one the install started with: the callback's
    // full URL without its query, whether it's mounted with app.Map (PathBase) or as an endpoint (Path).
    internal static string RedirectUri(HttpRequest request) =>
        $"{request.Scheme}://{request.Host.Value}{request.PathBase.Value}{request.Path.Value}";

    private static async Task Fail(HttpContext ctx, OAuthOptions settings, int status, string error)
    {
        if (settings.FailedRedirectUri is { } failed)
        {
            ctx.Response.Redirect(failed);
            return;
        }

        ctx.Response.StatusCode = status;
        if (error is not null)
        {
            await ctx.Response.WriteAsync(error);
        }
    }

    private static async Task<WorkspaceInstaller> Installer(OAuthClient client, OAuthClient.OAuthUser installer,
        ILogger logger)
    {
        if (installer?.Id is null)
        {
            return null;
        }

        if (installer.Access_Token is null || !(installer.Scope ?? "").Split(',').Contains("openid"))
        {
            return new WorkspaceInstaller(installer.Id);
        }

        try
        {
            var userInfo = await client.OpenIdUserInfo(installer.Access_Token);
            if (userInfo is { Ok: true })
            {
                return new WorkspaceInstaller(installer.Id, userInfo.Email, userInfo.Email_Verified, userInfo.Name);
            }

            logger.LogWarning("openid.connect.userInfo failed for installer {InstallerUserId}: {Error}", installer.Id, userInfo?.Error);
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "openid.connect.userInfo failed for installer {InstallerUserId}", installer.Id);
        }

        return new WorkspaceInstaller(installer.Id);
    }

    // `state` is opaque to this library — only the app that sent it knows what it means, so it
    // rides back to that app's own success page untouched rather than being interpreted here.
    internal static string SuccessRedirect(string successRedirectUri, string state) =>
        string.IsNullOrEmpty(state)
            ? successRedirectUri
            : QueryHelpers.AddQueryString(successRedirectUri, "state", state);
}
