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

/// <param name="Installer">
///     The installing user's Slack profile. Only set when the install requested the <c>openid</c> user scope
///     (together with <c>email</c> and <c>profile</c> for those claims).
/// </param>
public record Workspace(string TeamId, string TeamName, string Token, string InstallerUserId = null, InstallerIdentity Installer = null);

public record InstallerIdentity(string Email, bool EmailVerified, string Name);
