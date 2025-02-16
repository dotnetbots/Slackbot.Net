using Microsoft.Extensions.Logging;

namespace Slackbot.Net.Tests.Helpers;

public class XUnitLogger<T>(ITestOutputHelper helper) : ILogger<T>
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        var s = formatter(state, exception);
        helper.WriteLine(s);
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
