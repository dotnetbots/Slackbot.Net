using Slackbot.Net.SlackClients.Http.Models.Responses.ConversationsList;
using Slackbot.Net.SlackClients.Http.Models.Responses.ConversationsRepliesResponse;

namespace Slackbot.Net.SlackClients.Http.Models.Responses.ConversationsHistoryResponse;

public class ConversationsHistoryResponse : Response
{
    public Message[] Messages { get; set; }
    public bool Has_More { get; set; }
    public ResponseMetadata Response_Metadata { get; set; }
}
