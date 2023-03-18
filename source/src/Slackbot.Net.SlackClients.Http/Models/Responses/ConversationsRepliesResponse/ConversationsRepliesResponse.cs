namespace Slackbot.Net.SlackClients.Http.Models.Responses.ConversationsRepliesResponse;

public class ConversationsRepliesResponse : Response
{
    public Message[] Messages { get; set; }
}

public class Message
{
    public string Type { get; set; }
    public string User { get; set; }
    public string Text { get; set; }
    public string Thread_Ts { get; set; }
    public string Parent_User_id { get; set; }
    public string Ts { get; set; }
}