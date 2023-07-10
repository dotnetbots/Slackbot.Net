namespace Slackbot.Net.Azure.Functions.Abstractions;

public interface IUninstall
{
    Task OnUninstalled(string teamId, string teamName);
}
