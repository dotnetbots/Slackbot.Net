namespace Slackbot.Net.SlackClients.Http.Models.Requests.AssistantThreadsSetSuggestedPrompts;

public class AssistantThreadsSetSuggestedPromptsRequest
{
    public string Channel_Id { get; set; }
    public string Thread_Ts { get; set; }
    public AssistantThreadPrompt[] Prompts { get; set; }
    public string Title { get; set; }
}

public class AssistantThreadPrompt
{
    public string Title { get; set; }
    public string Message { get; set; }
}
