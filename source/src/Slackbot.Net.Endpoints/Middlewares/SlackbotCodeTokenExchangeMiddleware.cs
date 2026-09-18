using Microsoft.AspNetCore.Http;
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
        logger.LogInformation("Installing!");
        var redirect_uri = new Uri($"{ctx.Request.Scheme}://{ctx.Request.Host.Value}{ctx.Request.PathBase.Value}");
        var code = ctx.Request.Query["code"].FirstOrDefault();

        if (code == null)
        {
            await next(ctx);
        }

        var response = await oAuthAccessClient.OAuthAccessV2(new OAuthClient.OauthAccessV2Request(
            code,
            options.Value.CLIENT_ID,
            options.Value.CLIENT_SECRET,
            redirect_uri.ToString()
        ));

        if (response.Ok)
        {
            logger.LogInformation($"Oauth response! ok:{response.Ok}");
            await installationHandler.Install(new Workspace
            (
                response.Team.Id,
                response.Team.Name,
                response.Access_Token
            ));

            var state = ctx.Request.Query["state"].FirstOrDefault();
            ctx.Response.Redirect(ResolveRedirectUri(state, options.Value.SuccessRedirectUri));
        }
        else
        {
            logger.LogError($"Bad Oauth response! {response}");
            ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await ctx.Response.WriteAsync(response.Error);
        }
    }

    /// <summary>
    ///     Slack round-trips the OAuth `state` parameter untouched, so an app can use it to carry
    ///     the page the install was started from. Only site-relative paths are honored, and they are
    ///     resolved against <see cref="OAuthOptions.SuccessRedirectUri" />'s origin so the user stays
    ///     on the same site. Anything else falls back to <see cref="OAuthOptions.SuccessRedirectUri" />.
    /// </summary>
    private static string ResolveRedirectUri(string state, string successRedirectUri)
    {
        if (!IsLocalUrl(state))
        {
            return successRedirectUri;
        }

        if (Uri.TryCreate(successRedirectUri, UriKind.Absolute, out var successUri) &&
            (successUri.Scheme == Uri.UriSchemeHttp || successUri.Scheme == Uri.UriSchemeHttps))
        {
            return new Uri(successUri, state).AbsoluteUri;
        }

        return state;
    }

    /// <summary>
    ///     Accepts site-relative urls like `/foo`, but not absolute (`https://host/foo`) or
    ///     protocol-relative (`//host`, `/\host`) ones, which would turn the callback into an open
    ///     redirect.
    /// </summary>
    private static bool IsLocalUrl(string url)
    {
        if (string.IsNullOrEmpty(url) || url[0] != '/')
        {
            return false;
        }

        if (url.Length > 1 && (url[1] == '/' || url[1] == '\\'))
        {
            return false;
        }

        return !url.Any(char.IsControl);
    }
}
