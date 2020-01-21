using System.Threading.Tasks;

namespace Slackbot.Net.Abstractions.Publishers
{
    public interface IPublisherBuilder
    {
        Task<IPublisher> Build(string slackTeamId);
    }
}