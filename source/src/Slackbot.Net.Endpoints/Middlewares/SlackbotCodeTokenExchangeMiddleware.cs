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

            var stateTheAppSent = ctx.Request.Query["state"].FirstOrDefault();
            ctx.Response.Redirect(SuccessRedirect(options.Value.SuccessRedirectUri, stateTheAppSent));
        }
        else
        {
            logger.LogError($"Bad Oauth response! {response}");
            ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await ctx.Response.WriteAsync(response.Error);
        }
    }

    // `state` is opaque to this library — only the app that sent it knows what it means, so it
    // rides back to that app's own success page untouched rather than being interpreted here.
    internal static string SuccessRedirect(string successRedirectUri, string state) =>
        string.IsNullOrEmpty(state)
            ? successRedirectUri
            : QueryHelpers.AddQueryString(successRedirectUri, "state", state);
}
