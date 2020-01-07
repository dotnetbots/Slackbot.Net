using System;

namespace Slackbot.Net.SlackClients.Rtm.Exceptions
{
    public class HandshakeException : Exception
    {
        public HandshakeException(string message) : base(message)
        { }
    }
}