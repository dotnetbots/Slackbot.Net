using Slackbot.Net.Abstractions.Hosting;

public class MyTokenStore : ITokenStore
{
    private readonly List<Workspace> _workspaces;
    public string SlackToken = Environment.GetEnvironmentVariable("Slackbot_SlackApiKey_BotUser");

    public MyTokenStore()
    {
        _workspaces = new List<Workspace>()
        {
            new Workspace {
                Token = SlackToken,
                TeamId = "T0EC3DG3A"
            }
        };
    }

    public Task<IEnumerable<string>> GetTokens()
    {
        return Task.FromResult(_workspaces.Select(c => c.Token));
    }

    public Task<string> GetTokenByTeamId(string teamId)
    {
        return Task.FromResult(_workspaces.First(t => t.TeamId == teamId).Token);
    }

    public Task Delete(string token)
    {
        var workspaceToRemove = _workspaces.First(w => w.Token == token);
        _workspaces.Remove(workspaceToRemove);
        return Task.CompletedTask;
    }

    private class Workspace
    {
        public string Token { get; set; }
        public string TeamId { get; set; }
    }
}
