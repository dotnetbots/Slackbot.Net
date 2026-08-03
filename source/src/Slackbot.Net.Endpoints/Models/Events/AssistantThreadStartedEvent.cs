namespace Slackbot.Net.Endpoints.Models.Events;

public class AssistantThreadStartedEvent : SlackEvent
{
    public AssistantThread Assistant_Thread { get; set; }
    public string Event_Ts { get; set; }
}

public class AssistantThread
{
    public string User_Id { get; set; }
    public string Channel_Id { get; set; }
    public string Thread_Ts { get; set; }
    public AssistantThreadContext Context { get; set; }
}

public class AssistantThreadContext
{
    public string Channel_Id { get; set; }
    public string Team_Id { get; set; }
    public string Enterprise_Id { get; set; }
}
