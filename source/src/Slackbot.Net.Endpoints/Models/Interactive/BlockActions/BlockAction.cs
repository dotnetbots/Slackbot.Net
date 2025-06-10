using Slackbot.Net.Endpoints.Models.Interactive.MessageActions;
using Slackbot.Net.Models.BlockKit;

namespace Slackbot.Net.Endpoints.Models.Interactive.BlockActions;

public class BlockActionInteraction : Interaction
{
    public Team Team { get; set; }
    public User User { get; set; }
    public Channel Channel { get; set; }
    public Message Message { get; set; }
    public State State { get; set; }
    public IEnumerable<ActionsBlock> Actions { get; set; }
}

public class State
{
    public Dictionary<string, Dictionary<string, SlackInputValue>> Values { get; set; }
}

public class SlackInputValue
{
    public string Type { get; set; }
    public Option SelectedOption { get; set; }
    public string Value { get; set; }
}
