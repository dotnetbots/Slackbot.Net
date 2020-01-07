using System.ComponentModel.DataAnnotations;
using Slackbot.Net.Utilities;
using Slackbot.Net.Validations;

namespace Slackbot.Net.Configuration
{
    public class CronOptions
    {
        public CronOptions()
        {
            // Defaults to Oslo time
            if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
            {
                TimeZoneId = "Europe/Oslo";
            }
            else
            {
                TimeZoneId = "Central European Standard Time";
            }
        }

        [Required]
        public string Cron
        {
            get;
            set;
        }


        /// <summary>
        /// Default: Europe/oslo
        /// </summary>
        [RequiredValidTimeZone]
        public string TimeZoneId
        {
            get;
            set;
        }
    }
}