namespace Slackbot.Net.Endpoints.Models.Events;

public class EventMetaData
{
    public string Token { get; set; }
    public string Team_Id { get; set; }
    public string Type { get; set; }
    public string[] AuthedUsers { get; set; }
    public string Event_Id { get; set; }
    public long Event_Time { get; set; }
    public long Api_App_Id { get; set; }
    public Authorization[] Authorizations { get; set; }
    public bool Is_Ext_Shared_Channel { get; set; }
}

public class Authorization
{
    public string Enterprise_Id { get; set; }
    public string Team_Id { get; set; }
    public string User_Id { get; set; }
    public bool Is_Bot { get; set; }
    public bool Is_Enterprise_Install { get; set; }
}
