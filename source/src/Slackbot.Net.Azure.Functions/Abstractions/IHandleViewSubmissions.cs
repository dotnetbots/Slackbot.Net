using Slackbot.Net.Azure.Functions.Models.Interactive.ViewSubmissions;

namespace Slackbot.Net.Azure.Functions.Abstractions;

public interface IHandleViewSubmissions
{
    Task<EventHandledResponse> Handle(ViewSubmission payload);
}
