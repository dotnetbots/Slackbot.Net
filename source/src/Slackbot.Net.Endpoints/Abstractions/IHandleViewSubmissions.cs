using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Endpoints.Interactive.ViewSubmissions;

namespace Slackbot.Net.Endpoints.Abstractions
{
    public interface IHandleViewSubmissions
    {
        Task<HandleResponse> Handle(ViewSubmission payload);
    }
}