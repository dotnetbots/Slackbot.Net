using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Interactive.ViewSubmissions;

namespace HelloWorld.EventHandlers
{
    public class AppHomeViewSubmissionHandler : IHandleViewSubmissions
    {
        public Task<ViewSubmissionHandleResponse> Handle(ViewSubmission payload)
        {
            return Task.FromResult(new ViewSubmissionHandleResponse("YoLO!"));
        }
    }
}