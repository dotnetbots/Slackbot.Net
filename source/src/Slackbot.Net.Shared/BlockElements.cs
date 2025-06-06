namespace Slackbot.Net.Models.BlockKit;

public class SectionBlock : IBlock
{
    public string type { get; set; } = BlockTypes.Section;
    public string block_id { get; set; }
    public Text text { get; set; }
    public IElement accessory { get; set; }
    public Text[] fields { get; set; }
}
public class DividerBlock : IBlock
{
    public string type { get; set; } = BlockTypes.Divider;
    public string block_id { get; set; }
}
public class ImageBlock : IBlock
{
    public string type { get;set;} = BlockTypes.Image;
    public string block_id { get; set; }
    public Text title { get; set; }
    public string image_url { get; set; }
    public string alt_text { get; set; }
}
public class ActionsBlock : IBlock
{
    public string action_id { get; set; }
    public string type { get; set;} = BlockTypes.Actions;
    public string block_id { get; set; }
    public string value { get; set; }
    public IElement[] elements { get; set; }
}
public class ContextBlock : IBlock
{
    public string type { get; set;} = BlockTypes.Context;
    public string block_id { get; set; }
    public IElement[] elements { get; set; }
}

public class InputBlock : IBlock
{
    public string block_id { get; set; }
    public string type { get; set; } = BlockTypes.Input;
    public IElement element { get; set; }
    public Text label { get; set; }
    public bool dispatch_action { get; set; }
}

public class Text : IElement
{
    public string type { get; set; } = TextTypes.PlainText;
    public string text { get; set; }
    public bool? emoji { get; set; }
    public bool? verbatim { get; set; }
}

public class Option
{
    public Text text { get; set; }
    public string value { get; set; }
}

public class OptionGroups
{
    public Text label { get; set; }
    public Option[] options { get; set; }
}

public class Confirm
{
    public Text title { get; set; }
    public Text text { get; set; }
    public Text confirm { get; set; }
    public Text deny { get; set; }
}

public class Element : IElement
{
    public string type { get; set; }
    public string action_id { get; set; }
    public Text text { get; set; }
    public string value { get; set; }
    public Text placeholder { get; set; }
    public Option[] options { get; set; }
    public OptionGroups[] option_groups { get; set; }
    public string image_url { get; set; }
    public string alt_text { get; set; }
    public string url { get; set; }
    public string initial_date { get; set; }
    public string initial_user { get; set; }
    public string initial_channel { get; set; }
    public string initial_conversation { get; set; }
    public string initial_option { get; set; }
    public int? min_query_length { get; set; }
    public Confirm confirm { get; set; }
    public string style { get; set; }
}

public class PlainTextInputElement : IElement
{
    public string type { get; set; } = ElementTypes.PlainTextInput;
    public string initial_value { get; set; }
    public string action_id { get; set; }
    public Text placeholder { get; set; }
}

public class ImageElement : IElement
{
    public string type { get; set;} = ElementTypes.Image;
    public string image_url { get; set; }
    public string alt_text { get; set; }
}
public class ButtonElement : IElement
{
    public string type { get; set;} = ElementTypes.Button;
    public string action_id { get; set; }
    public Text text { get; set; }
    public string value { get; set; }
    public Text placeholder { get; set; }
    public Option[] options { get; set; }
    public OptionGroups[] option_groups { get; set; }
    public string url { get; set; }
    public Confirm confirm { get; set; }
    public string style { get; set; }
}
public class StaticSelectElement : IElement
{
    public string type { get; set;} = ElementTypes.StaticSelect;
    public string action_id { get; set; }
    public Text placeholder { get; set; }
    public Option[] options { get; set; }
    public OptionGroups[] option_groups { get; set; }
    public string initial_option { get; set; }
    public Confirm confirm { get; set; }
}
public class ExternalSelectElement : IElement
{
    public string type { get; set;} = ElementTypes.ExternalSelect;
    public string action_id { get; set; }
    public Text placeholder { get; set; }
    public string initial_option { get; set; }
    public int min_query_length { get; set; }
    public Confirm confirm { get; set; }
}


public class UserSelectElement : IElement
{
    public string type { get; set;} = ElementTypes.UserSelect;
    public string action_id { get; set; }
    public Text placeholder { get; set; }
    public string initial_user { get; set; }
    public Confirm confirm { get; set; }
}
public class ConversationSelectElement : IElement
{
    public string type { get; set;} = ElementTypes.ChannelSelect;
    public string action_id { get; set; }
    public Text placeholder { get; set; }
    public string initial_conversation { get; set; }
    public Confirm confirm { get; set; }
}
public class ChannelSelectElement : IElement
{
    public string type { get; set;} = ElementTypes.ChannelSelect;
    public string action_id { get; set; }
    public Text placeholder { get; set; }
    public string initial_channel { get; set; }
    public Confirm confirm { get; set; }
}
public class OverflowElement : IElement
{
    public string type { get; set;} = ElementTypes.Overflow;
    public string action_id { get; set; }
    public Option[] options { get; set; }
    public Confirm confirm { get; set; }
}

public class DatePickerElement : IElement
{
    public string type { get; set; } = ElementTypes.DatePicker;
    public string action_id { get; set; }
    public Text placeholder { get; set; }
    public string initial_date { get; set; }
    public Confirm confirm { get; set; }
}

public class RadioButtonsElement : IElement
{
    public string type { get; set; } = ElementTypes.RadioButtons;
    public string action_id { get; set; }
    public Option[] options { get; set; }
    public string initial_option { get; set; }
    public Confirm confirm { get; set; }
    public bool focus_on_load { get; set; }
}

public class PlainTextElement : IElement
{
    public string type { get; set; } = ElementTypes.PlainTextInput;
    public string action_id { get; set; }
    public Text placeholder { get; set; }
}

public static class ButtonStyles
{
    public const string Primary = "primary";
    public const string Danger = "danger";
}

public static class BlockTypes
{
    public const string Section = "section";
    public const string Divider = "divider";
    public const string Actions = "actions";
    public const string Context = "context";
    public const string Image = "image";
    public const string Input = "input";
}

public static class TextTypes
{
    public const string Markdown = "mrkdwn";
    public const string PlainText = "plain_text";
}

public static class ElementTypes
{
    public const string Image = "image";
    public const string Button = "button";
    public const string StaticSelect = "static_select";
    public const string ExternalSelect = "external_select";
    public const string UserSelect = "users_select";
    public const string ChannelSelect = "channel_select";
    public const string ConversationSelect = "conversation_select";
    public const string Overflow = "overflow";
    public const string DatePicker = "datepicker";
    public const string PlainTextInput = "plain_text_input";
    public const string RadioButtons = "radio_buttons";
}

public interface IHaveType { string type { get; set; } }

public interface IElement : IHaveType { }
public interface IBlock : IHaveType { }
