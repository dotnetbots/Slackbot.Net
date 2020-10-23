using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CronBackgroundServices
{
    public interface IRecurringAction
    {
        /// <summary>
        /// The job to be executed at intervals defined by the Cron expression
        /// </summary>
        /// <returns></returns>
        Task Process(CancellationToken stoppingToken);
        
        /// <summary>
        /// The cron expression (including seconds) as defined by the Cronos library:
        /// See https://github.com/HangfireIO/Cronos#cron-format
        /// Ex: Every second: */1 * * * * *
        /// Ex: Every minute: 0 */1 * * * *
        /// Ex: Every midnight: 0 0 */1 * * *
        /// Ex: First of every month 0 0 0 1 * *
        /// </summary>
        /// <returns>A valid Cron Expression</returns>
        string Cron { get; }
        
        /// <summary>
        /// The TimeZone in which the Cron expression should be based on.
        /// Defaults to Europe/Oslo.
        /// 
        /// NB! Platform specific, so make sure it returns a valid timezoneId for the platform you're targeting!
        /// </summary>
        /// <returns>timezoneId</returns>
        string GetTimeZoneId()
        {
            return !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Europe/Oslo" : "Central European Standard Time";
        }
    }
}