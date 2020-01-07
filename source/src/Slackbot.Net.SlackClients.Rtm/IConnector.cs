using System.Threading.Tasks;

namespace Slackbot.Net.SlackClients.Rtm
{
    public interface IConnector
    {
        Task<IConnection> Connect();
    }
}