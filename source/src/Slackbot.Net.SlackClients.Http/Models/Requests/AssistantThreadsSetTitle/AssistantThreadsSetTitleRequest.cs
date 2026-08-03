namespace Slackbot.Net.SlackClients.Http.Models.Requests.AssistantThreadsSetTitle;

public class AssistantThreadsSetTitleRequest
{
    public string Channel_Id { get; set; }
    public string Thread_Ts { get; set; }
    public string Title { get; set; }
}
