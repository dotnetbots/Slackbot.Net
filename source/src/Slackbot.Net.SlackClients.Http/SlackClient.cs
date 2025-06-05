using Microsoft.Extensions.Logging;
using Slackbot.Net.SlackClients.Http.Extensions;
using Slackbot.Net.SlackClients.Http.Models.Requests.ChatPostEphemeral;
using Slackbot.Net.SlackClients.Http.Models.Requests.ChatPostMessage;
using Slackbot.Net.SlackClients.Http.Models.Requests.ChatUpdate;
using Slackbot.Net.SlackClients.Http.Models.Requests.FileUpload;
using Slackbot.Net.SlackClients.Http.Models.Requests.ViewPublish;
using Slackbot.Net.SlackClients.Http.Models.Responses;
using Slackbot.Net.SlackClients.Http.Models.Responses.ChatGetPermalink;
using Slackbot.Net.SlackClients.Http.Models.Responses.ChatPostMessage;
using Slackbot.Net.SlackClients.Http.Models.Responses.ConversationsList;
using Slackbot.Net.SlackClients.Http.Models.Responses.ConversationsRepliesResponse;
using Slackbot.Net.SlackClients.Http.Models.Responses.FileUpload;
using Slackbot.Net.SlackClients.Http.Models.Responses.UserProfile;
using Slackbot.Net.SlackClients.Http.Models.Responses.UsersList;
using Slackbot.Net.SlackClients.Http.Models.Responses.ViewPublish;

namespace Slackbot.Net.SlackClients.Http;

/// <inheritdoc/>
public class SlackClient : ISlackClient
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
            new KeyValuePair<string, string>("as_user", "true"),
            new KeyValuePair<string, string>("link_names", "true")

        };
        return await _client.PostParametersAsForm<ChatPostMessageResponse>(parameters, "chat.postMessage", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<ChatPostMessageResponse> ChatPostMessage(ChatPostMessageRequest postMessage)
    {
        return await _client.PostJson<ChatPostMessageResponse>(postMessage, "chat.postMessage", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<ChatPostMessageResponse> ChatPostEphemeralMessage(ChatPostEphemeralMessageRequest postMessage)
    {
        return await _client.PostJson<ChatPostMessageResponse>(postMessage, "chat.postEphemeral", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<ChatPostMessageResponse> ChatUpdate(ChatUpdateRequest postMessage)
    {
        return await _client.PostJson<ChatPostMessageResponse>(postMessage, "chat.update", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<ChatGetPermalinkResponse> ChatGetPermalink(string channel, string message_ts)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("channel", channel),
            new KeyValuePair<string, string>("message_ts", message_ts)
        };

        return await _client.PostParametersAsForm<ChatGetPermalinkResponse>(parameters, "chat.getPermalink", s => _logger.LogTrace(s));
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

        return await _client.PostParametersAsForm<ChatGetPermalinkResponse>(parameters, "reactions.add", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<UsersListResponse> UsersList()
    {
        return await _client.PostParametersAsForm<UsersListResponse>(null, "users.list", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<ConversationsListResponse> ConversationsListPublicChannels(int? limit = null, string cursor = null)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("types", "public_channel"),
            new KeyValuePair<string, string>("exclude_archived", "true"),
            new KeyValuePair<string, string>("limit", (limit ?? 200).ToString()),
        };
        if (cursor != null)
        {
            parameters.Add(new KeyValuePair<string, string>("cursor", cursor));
        }
        return await _client.PostParametersAsForm<ConversationsListResponse>(parameters, "conversations.list", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<ConversationsListResponse> ConversationsMembers(string channel)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("channel", channel)
        };
        return await _client.PostParametersAsForm<ConversationsListResponse>(parameters, "conversations.members", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<ConversationsRepliesResponse> ConversationsReplies(string channel, string ts, int? limit = null, string cursor = null)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("channel", channel),
            new KeyValuePair<string, string>("ts", ts),
            new KeyValuePair<string, string>("limit", (limit ?? 1000).ToString()),
            new KeyValuePair<string, string>("include_all_metadata", "true"),
        };
        return await _client.PostParametersAsForm<ConversationsRepliesResponse>(parameters, "conversations.replies", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<ConversationsOpenResponse> ConversationsOpen(string[] users)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("users", string.Join(",", users)),
        };
        return await _client.PostParametersAsForm<ConversationsOpenResponse>(parameters, "conversations.open", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<Response> AppsUninstall(string clientId, string clientSecret)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret)
        };
        return await _client.PostParametersAsForm<ConversationsListResponse>(parameters, "apps.uninstall", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<ViewPublishResponse> ViewPublish(ViewPublishRequest view)
    {
        return await _client.PostJson<ViewPublishResponse>(view, "views.publish", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<UserProfileResponse> UserProfile(string user)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("user", user),
        };
        return await _client.PostParametersAsForm<UserProfileResponse>(parameters, "users.profile.get", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<FileUploadResponse> FilesUpload(FileUploadRequest req)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("channels", req.Channels),
            new KeyValuePair<string, string>("title", req.Title),
            new KeyValuePair<string, string>("content", req.Content),
            new KeyValuePair<string, string>("filename", req.Filename),
            new KeyValuePair<string, string>("filetype", req.Filetype),
            new KeyValuePair<string, string>("initial_comment", req.Initial_Comment),
            new KeyValuePair<string, string>("thread_ts", req.Thread_Ts),
        };
        return await _client.PostParametersAsForm<FileUploadResponse>(parameters, "files.upload", s => _logger.LogTrace(s));
    }

    /// <inheritdoc/>
    public async Task<FileUploadResponse> FilesUpload(FileUploadMultiPartRequest req)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("channels", req.Channels),
            new KeyValuePair<string, string>("title", req.Title),
            new KeyValuePair<string, string>("filename", req.Filename),
        };

        if (req.Initial_Comment is { })
        {
            parameters.Add(new KeyValuePair<string, string>("initial_comment", req.Initial_Comment));
        }

        if (req.Thread_Ts is { })
        {
            parameters.Add(new KeyValuePair<string, string>("thread_ts", req.Thread_Ts));
        }

        return await _client.PostParametersAsMultiPartFormData<FileUploadResponse>(parameters, req.File, "files.upload", s => _logger.LogTrace(s));
    }


}
