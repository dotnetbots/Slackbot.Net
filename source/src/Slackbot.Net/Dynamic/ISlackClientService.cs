using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Http;

namespace Slackbot.Net.Dynamic
{
    public interface ISlackClientService
    {
        Task<ISlackClient> CreateClient(string teamId);
    }
}