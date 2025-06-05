using Slackbot.Net.Models.BlockKit;

namespace Slackbot.Net.SlackClients.Http.Models.Requests.ChatUpdate;

public class ChatUpdateRequest
{
    public string channel { get; set; }
    public string ts { get; set; }
    public string text { get; set; }
    public IBlock[] blocks { get; set; }
}
