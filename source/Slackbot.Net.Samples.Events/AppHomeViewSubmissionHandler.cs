using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Interactive.ViewSubmissions;

namespace Slackbot.Net.Samples.Events
{
    public class AppHomeViewSubmissionHandler : IHandleViewSubmissions
    {
        public Task<HandleResponse> Handle(ViewSubmission payload)
        {
            return Task.FromResult(new HandleResponse("YoLO!"));
        }
    }
}