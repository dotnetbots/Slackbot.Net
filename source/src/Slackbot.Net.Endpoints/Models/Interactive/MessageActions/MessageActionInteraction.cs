namespace Slackbot.Net.Endpoints.Models.Interactive.MessageActions;

public class MessageActionInteraction : Interaction
{
    public string Callback_Id { get; set; }
    public string Response_Url { get; set; }

    public Team Team { get; set; }
    public User User { get; set; }
    public Channel Channel { get; set; }

    public string Message_Ts { get; set; }

    public Message Message { get; set; }
}

public class Team
{
    public string Id { get; set; }
}

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
}

public class Channel
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class Message
{
    public string Text { get; set; }
    public string User { get; set; }
    public string Ts { get; set; }
    public string Thread_Ts { get;set; }
}