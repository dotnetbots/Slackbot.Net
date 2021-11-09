using Microsoft.Extensions.Logging;

namespace Slackbot.Net.Tests.Helpers
{
    public class XUnitLogger<T> : ILogger<T>
    {
        private readonly ITestOutputHelper _helper;

        public XUnitLogger(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var s = formatter(state, exception);
            _helper.WriteLine(s);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}