using System.Threading.Tasks;

namespace Slackbot.Net.Endpoints.Interactive
{
    public interface IHandleInteractiveActions
    {
        Task<object> RespondToSlackInteractivePayload(IncomingInteractiveMessage incoming);
    }
}