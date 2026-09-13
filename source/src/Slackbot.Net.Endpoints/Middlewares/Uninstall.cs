using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Middlewares;

public class Uninstall
{
    private readonly ILogger<Uninstall> _logger;
    public Uninstall(RequestDelegate next, ILogger<Uninstall> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var installationHandler = context.RequestServices.GetService<IWorkspaceInstallationHandler>() ??
                         new NoopWorkspaceInstallationHandler(context.RequestServices.GetService<ILogger<NoopWorkspaceInstallationHandler>>() ??
                                            NullLogger<NoopWorkspaceInstallationHandler>.Instance);
        var metadata = context.Items[HttpItemKeys.EventMetadataKey] as EventMetaData;
        _logger.LogInformation($"Uninstalling team with TeamId: `{metadata.Team_Id}`");
        var deleted = await installationHandler.Uninstall(metadata.Team_Id);
        if (deleted is null)
        {
            _logger.LogWarning(
                "Workspace installation handler returned null for '{TeamId}'.",
                metadata.Team_Id);
        }
        else
        {
            _logger.LogInformation($"Uninstalled team with TeamId: `{metadata.Team_Id}`");
        }

        context.Response.StatusCode = 200;
    }

    public static bool ShouldRun(HttpContext ctx)
    {
        return ctx.Items.ContainsKey(HttpItemKeys.EventTypeKey) &&
               (ctx.Items[HttpItemKeys.EventTypeKey].ToString() == EventTypes.AppUninstalled ||
                ctx.Items[HttpItemKeys.EventTypeKey].ToString() == EventTypes.TokensRevoked);
    }
}

public class NoopWorkspaceInstallationHandler(ILogger<NoopWorkspaceInstallationHandler> logger) : IWorkspaceInstallationHandler
{
    public Task<Workspace?> Uninstall(string teamId)
    {
        logger.LogDebug("No-op. Returning null for uninstalling workspace!");
        return Task.FromResult<Workspace?>(null);
    }

    public Task Install(Workspace workspace)
    {
        logger.LogDebug("No-op. Not storing workspace!");
        return Task.CompletedTask;
    }
}
