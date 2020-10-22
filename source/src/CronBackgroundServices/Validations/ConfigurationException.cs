using System;

namespace CronBackgroundServices.Validations
{
    internal class ConfigurationException : Exception
    {
        public ConfigurationException(string s) : base(s)
        {
        }
    }
}