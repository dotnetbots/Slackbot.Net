using System;

namespace Slackbot.Net.SlackClients.Rtm.Exceptions
{
    public class TokenRevokedException : Exception
    {
        public TokenRevokedException(string msg) : base(msg)
        {
            
        }
    }
}