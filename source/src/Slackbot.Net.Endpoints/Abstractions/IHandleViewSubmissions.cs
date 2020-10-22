using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Models.Interactive.ViewSubmissions;

namespace Slackbot.Net.Endpoints.Abstractions
{
    public interface IHandleViewSubmissions
    {
        Task<ViewSubmissionHandleResponse> Handle(ViewSubmission payload);
    }

    public class ViewSubmissionHandleResponse
    {
        public ViewSubmissionHandleResponse(string message)
        {
            HandledMessage = message;
        }

        public string HandledMessage
        {
            get;
            set;
        }
    }
    
    
}