using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Slackbot.Net.SlackClients.Http.Extensions;
using Slackbot.Net.SlackClients.Http.Models.Requests.ChatPostMessage;
using Slackbot.Net.SlackClients.Http.Models.Responses;
using Slackbot.Net.SlackClients.Http.Models.Responses.ChatGetPermalink;
using Slackbot.Net.SlackClients.Http.Models.Responses.ChatPostMessage;
using Slackbot.Net.SlackClients.Http.Models.Responses.UsersList;

namespace Slackbot.Net.SlackClients.Http
{
    /// <inheritdoc/>
    internal class SlackClient : ISlackClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ISlackClient> _logger;

        public SlackClient(HttpClient client, ILogger<ISlackClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ChatPostMessageResponse> ChatPostMessage(string channel, string text)
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("channel", channel),
                new KeyValuePair<string, string>("text", text),
            };
            return await _client.PostParametersAsForm<ChatPostMessageResponse>(parameters, "chat.postMessage", s => _logger.LogTrace(s));
        }

        /// <inheritdoc/>
        public async Task<ChatPostMessageResponse> ChatPostMessage(ChatPostMessageRequest postMessage)
        {
            return await _client.PostJson<ChatPostMessageResponse>(postMessage, "chat.postMessage", s => _logger.LogTrace(s));
        }

        /// <inheritdoc/>
        public async Task<ChatGetPermalinkResponse> ChatGetPermalink(string channel, string message_ts)
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("channel", channel),
                new KeyValuePair<string, string>("message_ts", message_ts)
            };
          
            return await _client.PostParametersAsForm<ChatGetPermalinkResponse>(parameters,"chat.getPermalink", s => _logger.LogTrace(s));
        }

        /// <inheritdoc/>
        public async Task<Response> ReactionsAdd(string name, string channel, string timestamp)
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("channel", channel),
                new KeyValuePair<string, string>("timestamp", timestamp)
            };
          
            return await _client.PostParametersAsForm<ChatGetPermalinkResponse>(parameters,"reactions.add", s => _logger.LogTrace(s));
        }

        /// <inheritdoc/>
        public async Task<UsersListResponse> UsersList()
        {
            return await _client.PostParametersAsForm<UsersListResponse>(null,"users.list", s => _logger.LogTrace(s));
        }
    }
}