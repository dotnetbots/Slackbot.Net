namespace Slackbot.Net.SlackClients.Http.Models.Requests.OAuthAccess
{
    public class OauthAccessRequest
    {
        /// <summary>
        /// Required
        /// </summary>
        public string ClientId { get; set; }
        
        /// <summary>
        /// Required
        /// </summary>
        public string ClientSecret { get; set; }
        
        /// <summary>
        /// Required
        /// </summary>
        public string Code { get; set; }
        
        public string RedirectUri { get; set; }
        public bool? SingleChannel { get; set; }
    }
}