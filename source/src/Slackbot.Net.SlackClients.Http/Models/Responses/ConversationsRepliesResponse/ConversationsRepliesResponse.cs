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

    // Populated on bot/app-authored messages. Reliable bot detection: Bot_Id != null.
    // See https://docs.slack.dev/reference/events/message/bot_message
    public string Bot_Id { get; set; }
    public string App_Id { get; set; }
    public BotProfile Bot_Profile { get; set; }
}

public class BotProfile
{
    public string Id { get; set; }
    public string App_Id { get; set; }
    public string Name { get; set; }
}