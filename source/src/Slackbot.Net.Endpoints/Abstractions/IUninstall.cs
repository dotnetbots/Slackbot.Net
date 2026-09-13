namespace Slackbot.Net.Endpoints.Abstractions;

[Obsolete("Put post-uninstall logic directly in your IWorkspaceInstallationHandler.Uninstall implementation instead. IUninstall will be removed in a future version.")]
public interface IUninstall
{
    Task OnUninstalled(string teamId, string teamName);
}
