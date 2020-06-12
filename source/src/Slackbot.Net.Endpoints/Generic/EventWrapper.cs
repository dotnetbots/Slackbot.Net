namespace Slackbot.Net.Endpoints.Generic
{
    public class EventWrapper
    {
        public string Token { get; set; }
        public string Team_Id { get; set; }
        public object Event { get; set; }
        public string Type { get; set; }
        public string[] AuthedUsers { get; set; }
        public string Event_Id { get; set; }
        public string Event_Time { get; set; }
    }
}