namespace Slackbot.Net.Abstractions.Hosting;

/// <summary>
///     Reacts to a Slack app's installation lifecycle: install (OAuth success) and
///     uninstall (the `app_uninstalled` / `tokens_revoked` events), giving you a hook to
///     persist or remove the workspace's access token however you see fit.
/// </summary>
public interface IWorkspaceInstallationHandler
{
    /// <summary>Called when a workspace completes the OAuth installation flow for your app.</summary>
    Task Install(Workspace workspace);

    /// <summary>
    ///     Called when a workspace uninstalls your app or revokes its tokens.
    ///     Return the removed <see cref="Workspace"/>, or null if none was found for <paramref name="teamId"/>.
    /// </summary>
    Task<Workspace?> Uninstall(string teamId);
}

public record Workspace(string TeamId, string TeamName, string Token);
