namespace Slackbot.Net.SlackClients.Http.Models.Responses.ConversationsInfo;

public class ConversationsInfoResponse : Response
{
    public ConversationInfo Channel { get; set; }
}

public class ConversationInfo
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Is_Channel { get; set; }
    public bool Is_Archived { get; set; }

    // Only populated when include_num_members=true is passed.
    public int? Num_Members { get; set; }
}
