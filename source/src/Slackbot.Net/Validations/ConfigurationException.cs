using System;

namespace Slackbot.Net.Validations
{
    internal class ConfigurationException : Exception
    {
        public ConfigurationException(string s) : base(s)
        {
        }
    }
}