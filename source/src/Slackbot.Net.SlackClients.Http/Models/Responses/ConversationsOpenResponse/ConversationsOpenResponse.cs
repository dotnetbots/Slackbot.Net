using Slackbot.Net.SlackClients.Http.Models.Responses;

public class ConversationsOpenResponse : Response
{
    public Channel channel { get; set; }
}

public class Channel
{
    public string id { get; set; }
}
