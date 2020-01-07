using System.Threading.Tasks;

namespace Slackbot.Net.Endpoints.Interactive
{
    public interface IRespond
    {
        Task<RespondResult> Respond(string responseUrl, string responseText);
    }
}