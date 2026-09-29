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
    /// </summary>
    Task Uninstall(string teamId);
}

/// <param name="Installer">The Slack user who installed the app, when Slack reports one.</param>
public record Workspace(string TeamId, string TeamName, string Token, WorkspaceInstaller Installer = null);

/// <summary>
///     The Slack user who installed the app. <see cref="Email"/>, <see cref="EmailVerified"/> and <see cref="Name"/>
///     are only set when the install requested the <c>openid</c> user scope (with <c>email</c> and <c>profile</c>).
/// </summary>
public record WorkspaceInstaller(string UserId, string Email = null, bool EmailVerified = false, string Name = null);
