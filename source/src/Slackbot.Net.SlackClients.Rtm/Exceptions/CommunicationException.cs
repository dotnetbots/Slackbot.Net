using System;

namespace Slackbot.Net.SlackClients.Rtm.Exceptions
{
    public class CommunicationException : Exception
    {
        public CommunicationException(string message) : base(message)
        { }
    }
}