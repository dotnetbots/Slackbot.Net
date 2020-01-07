using System.Threading.Tasks;

namespace Slackbot.Net.Abstractions.Publishers
{
    public interface IPublisher
    {
        Task Publish(Notification notification);
    }
}