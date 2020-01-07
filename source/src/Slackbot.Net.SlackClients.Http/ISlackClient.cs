using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Http.Models.Requests.ChatPostMessage;
using Slackbot.Net.SlackClients.Http.Models.Responses;
using Slackbot.Net.SlackClients.Http.Models.Responses.ChatGetPermalink;
using Slackbot.Net.SlackClients.Http.Models.Responses.ChatPostMessage;
using Slackbot.Net.SlackClients.Http.Models.Responses.UsersList;

namespace Slackbot.Net.SlackClients.Http
{
    /// <summary>
    /// A slack client for endpoints that require a bot token (no human credentials)
    /// </summary>
    public interface ISlackClient
    {
        /// <summary>
        /// Scopes required: `chat:write`
        /// </summary>
        /// <remarks>https://api.slack.com/methods/chat.postMessage</remarks>
        Task<ChatPostMessageResponse> ChatPostMessage(string channel, string text);

        /// <summary>
        /// Scopes required: `chat:write`
        /// </summary>
        /// <remarks>https://api.slack.com/methods/chat.postMessage</remarks>
        Task<ChatPostMessageResponse> ChatPostMessage(ChatPostMessageRequest postMessage);

        /// <summary>
        /// Scopes required: no scopes required
        /// </summary>
        /// <remarks>https://api.slack.com/methods/chat.getPermalink</remarks>
        Task<ChatGetPermalinkResponse> ChatGetPermalink(string channel, string message_ts);

        /// <summary>
        /// Scopes required: `reactions:write`
        /// </summary>
        /// <remarks>https://api.slack.com/methods/reactions.add</remarks>
        Task<Response> ReactionsAdd(string name, string channel, string timestamp);

        /// <summary>
        /// Scopes required: `users:read`
        /// </summary>  
        /// <remarks>https://api.slack.com/methods/users.list</remarks>
        Task<UsersListResponse> UsersList();
    }
}