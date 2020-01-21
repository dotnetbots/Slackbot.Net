using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slackbot.Net.Abstractions.Handlers
{
    /// <summary>
    /// Provider of tokens from all workspaces that have installed your distributed Slack app
    /// </summary>
    public interface ITokenStore
    {
        Task<IEnumerable<string>> GetTokens();
        Task<string> GetTokenByTeamId(string teamId);
    }
}