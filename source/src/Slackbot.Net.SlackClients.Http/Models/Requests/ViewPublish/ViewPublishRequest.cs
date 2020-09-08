namespace Slackbot.Net.SlackClients.Http.Models.Requests.ViewPublish
{
    public class ViewPublishRequest
    {
        public ViewPublishRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
        public View View { get; set; }
    }
}