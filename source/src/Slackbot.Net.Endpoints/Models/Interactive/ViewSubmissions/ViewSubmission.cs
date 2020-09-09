
using Newtonsoft.Json.Linq;

namespace Slackbot.Net.Endpoints.Interactive.ViewSubmissions
{
    public class ViewSubmission : Interaction
    {
        public Team Team { get; set; }
        public User User { get; set; }
        
        public string ViewId { get; set; }
        public JObject ViewState { get; set; }
    }
}