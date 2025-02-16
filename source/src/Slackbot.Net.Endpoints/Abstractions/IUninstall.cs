namespace Slackbot.Net.Endpoints.Abstractions;

public interface IUninstall
{
    Task OnUninstalled(string teamId, string teamName);
}
