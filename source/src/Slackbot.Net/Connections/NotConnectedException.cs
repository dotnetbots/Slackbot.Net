using System;

namespace Slackbot.Net.Connections
{
    public class NotConnectedException : Exception
    {
        public NotConnectedException(string msg) : base(msg)
        {
            
        }
    }
}