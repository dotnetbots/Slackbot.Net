using System;
using System.ComponentModel.DataAnnotations;

namespace Slackbot.Net.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    internal class RequiredValidTimeZoneAttribute : ValidationAttribute
    {
        internal static string ErrorMsg = "Invalid value for current OS. See Linux/Mac: https://en.wikipedia.org/wiki/List_of_tz_database_time_zones or Windows: https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-time-zones";

        public RequiredValidTimeZoneAttribute() : base(ErrorMsg)
        {

        }

        public override bool IsValid(object value)
        {
            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(value.ToString());
                return tz != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}