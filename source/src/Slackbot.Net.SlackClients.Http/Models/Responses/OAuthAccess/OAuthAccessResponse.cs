namespace Slackbot.Net.SlackClients.Http.Models.Responses.OAuthAccess
{
    public class OAuthAccessResponse : Response
    {
        public string AccessToken { get; set; }
        public string Scope { get; set; }
    }
}