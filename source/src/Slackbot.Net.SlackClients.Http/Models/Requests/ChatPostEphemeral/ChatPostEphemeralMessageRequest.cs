using Slackbot.Net.Models.BlockKit;

namespace Slackbot.Net.SlackClients.Http.Models.Requests.ChatPostEphemeral;

public class ChatPostEphemeralMessageRequest
{
    public string Channel { get; set; }
    public string Text { get; set; }
    public string User { get; set; }

    public bool Link_Names { get; set; } = true;
    public string thread_ts { get; set; }
    public IBlock[] Blocks { get; set; }
}