using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slackbot.Net.Abstractions.Handlers
{
    public interface ITokenStore
    {
        Task<IEnumerable<string>> GetTokens();
    }
}