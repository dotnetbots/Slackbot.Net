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
