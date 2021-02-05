using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Interactive.ViewSubmissions;

namespace HelloWorld.EventHandlers
{
    public class AppHomeViewSubmissionHandler : IHandleViewSubmissions
    {
        public Task<EventHandledResponse> Handle(ViewSubmission payload)
        {
            return Task.FromResult(new EventHandledResponse("YoLO!"));
        }
    }
}