using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Middlewares;

public class Uninstall
{
    private readonly ILogger<Uninstall> _logger;
    private readonly ITokenStore _tokenStore;
    private readonly IUninstall _uninstaller;

    public Uninstall(RequestDelegate next, ILogger<Uninstall> logger, IServiceProvider provider)
    {
        _logger = logger;
        _uninstaller = provider.GetService<IUninstall>() ??
                       new NoopUninstaller(provider.GetService<ILogger<NoopUninstaller>>());
        _tokenStore = provider.GetService<ITokenStore>() ??
                      new NoopTokenStore(provider.GetService<ILogger<NoopTokenStore>>());
    }

    public async Task Invoke(HttpContext context)
    {
        var metadata = context.Items[HttpItemKeys.EventMetadataKey] as EventMetaData;
        _logger.LogInformation($"Deleting team with TeamId: `{metadata.Team_Id}`");
        var deleted = await _tokenStore.Delete(metadata.Team_Id);
        if (deleted is null)
        {
            _logger.LogWarning(
                "Token store returned null for '{TeamId}'. Will not trigger registered OnUninstalled handlers. ",
                metadata.Team_Id);
        }
        else
        {
            await _uninstaller.OnUninstalled(deleted?.TeamId, deleted?.TeamName);
            _logger.LogInformation($"Deleted team with TeamId: `{metadata.Team_Id}`");
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

public class NoopUninstaller(ILogger<NoopUninstaller> logger) : IUninstall
{
    public Task OnUninstalled(string teamId, string teamName)
    {
        logger.LogDebug("No OnUninstall function registered. No-op.");
        return Task.CompletedTask;
    }
}

public class NoopTokenStore(ILogger<NoopTokenStore> logger) : ITokenStore
{
    public Task<Workspace> Delete(string teamId)
    {
        logger.LogDebug("No-op. Returning null for deleting workspace!");
        return Task.FromResult<Workspace>(null);
    }

    public Task Insert(Workspace slackTeam)
    {
        logger.LogDebug("No-op. Not storing workspace!");
        return Task.CompletedTask;
    }
}
