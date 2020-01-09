using System.Collections.Generic;

namespace Slackbot.Net.SlackClients.Http.Models.Responses.ConversationsList
{
    public class ConversationsListResponse : Response
    {
        public IEnumerable<Conversation> Channels { get; set; }
    }
    
    public class Conversation
    {
        public string Name;
        public string Id;
        public bool Is_Channel;
        public bool Is_Group;
        public bool Is_Im;
        public bool Is_General;
        public bool Is_Archived;
    }
}