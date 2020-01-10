using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Http.Models.Requests.OAuthAccess;
using Slackbot.Net.SlackClients.Http.Models.Responses.OAuthAccess;

namespace Slackbot.Net.SlackClients.Http
{
    public interface ISlackOAuthAccessClient
    {
        Task<OAuthAccessResponse> OAuthAccess(OauthAccessRequest req);
    }
}