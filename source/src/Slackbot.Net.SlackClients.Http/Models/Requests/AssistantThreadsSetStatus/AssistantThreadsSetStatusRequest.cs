namespace Slackbot.Net.SlackClients.Http.Models.Requests.AssistantThreadsSetStatus;

public class AssistantThreadsSetStatusRequest
{
    public string Channel_Id { get; set; }
    public string Thread_Ts { get; set; }
    public string Status { get; set; }
}
