namespace Slackbot.Net.Abstractions.Hosting;

/// <summary>
///     Provider of tokens from all workspaces that have installed your distributed Slack app
/// </summary>
public interface ITokenManager
{
    Task<Workspace> Delete(string teamId);
    Task Insert(Workspace slackTeam);
}

public record Workspace(string TeamId, string TeamName, string Token);
