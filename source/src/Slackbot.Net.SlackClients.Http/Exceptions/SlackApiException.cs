using System;

namespace Slackbot.Net.SlackClients.Http.Exceptions
{
    public class SlackApiException : Exception
    {
        public SlackApiException(string s) : base(s)
        {
            
        }
    }
}