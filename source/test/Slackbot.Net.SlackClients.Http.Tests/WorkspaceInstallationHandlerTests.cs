using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Endpoints.Hosting;

namespace Slackbot.Net.Tests;

public class WorkspaceInstallationHandlerTests
{
    [Fact]
    public async Task FreshImplementerOfIWorkspaceInstallationHandlerWorks()
    {
        var services = new ServiceCollection();
        services.AddSlackBotEvents<FreshHandler>();
        var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        using var scope = provider.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService<IWorkspaceInstallationHandler>();
        var freshHandler = Assert.IsType<FreshHandler>(handler);

        await handler.Install(new Workspace("T1", "Team", "tok"));
        var deleted = await handler.Uninstall("T1");

        Assert.Single(freshHandler.Installed);
        Assert.Equal("T1", freshHandler.Installed[0].TeamId);
        Assert.Single(freshHandler.Uninstalled);
        Assert.Equal("T1", freshHandler.Uninstalled[0]);
        Assert.Equal("T1", deleted.TeamId);
    }

#pragma warning disable CS0618 // Type or member is obsolete
    [Fact]
    public async Task LegacyITokenStoreImplementerStillWorksThroughIWorkspaceInstallationHandler()
    {
        var services = new ServiceCollection();
        services.AddSlackBotEvents<LegacyStore>();
        var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        using var scope = provider.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService<IWorkspaceInstallationHandler>();
        var legacyStore = Assert.IsType<LegacyStore>(handler);

        await handler.Install(new Workspace("T1", "Team", "tok"));
        var deleted = await handler.Uninstall("T1");

        Assert.Single(legacyStore.Inserted);
        Assert.Equal("T1", legacyStore.Inserted[0].TeamId);
        Assert.Single(legacyStore.Deleted);
        Assert.Equal("T1", legacyStore.Deleted[0]);
        Assert.Equal("T1", deleted.TeamId);

        var legacyView = scope.ServiceProvider.GetRequiredService<ITokenStore>();
        Assert.Same(handler, legacyView);
    }

    private sealed class LegacyStore : ITokenStore
    {
        public List<Workspace> Inserted { get; } = [];
        public List<string> Deleted { get; } = [];

        public Task Insert(Workspace slackTeam)
        {
            Inserted.Add(slackTeam);
            return Task.CompletedTask;
        }

        public Task<Workspace> Delete(string teamId)
        {
            Deleted.Add(teamId);
            return Task.FromResult(new Workspace(teamId, "Team", "tok"));
        }
    }
#pragma warning restore CS0618

    private sealed class FreshHandler : IWorkspaceInstallationHandler
    {
        public List<Workspace> Installed { get; } = [];
        public List<string> Uninstalled { get; } = [];

        public Task Install(Workspace slackTeam)
        {
            Installed.Add(slackTeam);
            return Task.CompletedTask;
        }

        public Task<Workspace> Uninstall(string teamId)
        {
            Uninstalled.Add(teamId);
            return Task.FromResult(new Workspace(teamId, "Team", "tok"));
        }
    }
}
