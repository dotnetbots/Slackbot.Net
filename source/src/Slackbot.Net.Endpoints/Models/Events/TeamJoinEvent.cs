namespace Slackbot.Net.Endpoints.Models.Events;

public class TeamJoinEvent : SlackEvent
{
    public JoiningUser User { get; set; }
}

public class JoiningUser
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Real_Name { get; set; }
    public bool Is_Bot { get; set; }
    public bool Is_App_User { get; set; }
    public JoiningUserProfile Profile { get; set; }
}

public class JoiningUserProfile
{
    public string Title { get; set; }
    public string Phone { get; set; }
    public string Skype { get; set; }
    public string Real_Name { get; set; }
    public string Real_Name_Normalized { get; set; }
    public string Display_Name { get; set; }
    public string Display_Name_Normalized { get; set; }
    public string Status_Text { get; set; }
    public string Status_Emoji { get; set; }
    public int Status_Expiration { get; set; }
    public string Avatar_Hash { get; set; }
    public string First_Name { get; set; }
    public string Last_Name { get; set; }
    public string Image_24 { get; set; }
    public string Image_32 { get; set; }
    public string Image_48 { get; set; }
    public string Image_72 { get; set; }
    public string Image_192 { get; set; }
    public string Image_512 { get; set; }
}
