using Slackbot.Net.SlackClients.Http.Models.Responses.ConversationsList;

namespace Slackbot.Net.SlackClients.Http.Models.Responses.ConversationsMembers;

public class ConversationsMembersResponse : Response
{
    public IEnumerable<string> Members { get; set; }

    public ResponseMetadata Response_Metadata { get; set; }
}
