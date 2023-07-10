using Slackbot.Net.Models.BlockKit;

namespace Slackbot.Net.Azure.Functions.Models.Interactive.BlockActions;

public class BlockActionInteraction : Interaction
{
    public Team Team { get; set; }
    public User User { get; set; }
    public IEnumerable<ActionsBlock> Actions { get; set; }
}
