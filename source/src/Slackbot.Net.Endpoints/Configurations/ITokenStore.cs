namespace Slackbot.Net.Abstractions.Hosting;

/// <summary>
///     Obsolete: renamed to <see cref="IWorkspaceInstallationHandler"/>, with Insert/Delete
///     renamed to Install/Uninstall to match the Slack app-installation lifecycle events they
///     actually respond to. Kept for backwards compatibility: existing implementations of this
///     interface (their Insert/Delete methods unchanged) automatically satisfy
///     <see cref="IWorkspaceInstallationHandler"/> too via the forwarding below. Note: because the
///     forwarding methods are explicit implementations of IWorkspaceInstallationHandler, they're only
///     reachable via an IWorkspaceInstallationHandler-typed reference, not via ITokenStore itself.
/// </summary>
[Obsolete("ITokenStore has been renamed to IWorkspaceInstallationHandler, and its Insert/Delete methods renamed to Install/Uninstall to match the Slack app installation lifecycle. Implement IWorkspaceInstallationHandler instead. This interface will be removed in a future version.")]
public interface ITokenStore : IWorkspaceInstallationHandler
{
    Task<Workspace> Delete(string teamId);
    Task Insert(Workspace slackTeam);

    Task IWorkspaceInstallationHandler.Install(Workspace slackTeam) => Insert(slackTeam);
    Task<Workspace> IWorkspaceInstallationHandler.Uninstall(string teamId) => Delete(teamId);
}
