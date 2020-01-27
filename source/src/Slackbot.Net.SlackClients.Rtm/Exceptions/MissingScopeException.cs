using System;

namespace Slackbot.Net.SlackClients.Rtm.Exceptions
{
    public class MissingScopeException : Exception
    {
        public MissingScopeException(string msg) : base(msg)
        {
            
        }
    }
}