using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Slackbot.Net.Endpoints.Authentication;

internal class
    SlackbotEventsAuthenticationAuthenticationHandler : AuthenticationHandler<SlackbotEventsAuthenticationOptions>
{
    private const string TimestampHeaderName = "X-Slack-Request-Timestamp";
    private const string SignatureHeaderName = "X-Slack-Signature";

    private static readonly DateTime Seventies = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    private readonly string _signingSecret;

    public SlackbotEventsAuthenticationAuthenticationHandler(
        IOptionsMonitor<SlackbotEventsAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
        if (string.IsNullOrEmpty(options.CurrentValue.SigningSecret))
        {
            throw new ArgumentNullException(nameof(SlackbotEventsAuthenticationOptions.SigningSecret));
        }

        _signingSecret = options.CurrentValue.SigningSecret;
    }

    private static long Now => (long)DateTime.UtcNow.Subtract(Seventies).TotalSeconds;

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var headers = Request.Headers;

        var timestamp = headers[TimestampHeaderName].FirstOrDefault();
        var signature = headers[SignatureHeaderName].FirstOrDefault();
        var failures = new StringBuilder();
        if (timestamp == null)
        {
            failures.Append($"Missing header {TimestampHeaderName}");
        }

        if (signature == null)
        {
            failures.Append($"Missing header {TimestampHeaderName}");
        }

        if (timestamp is null || signature == null)
        {
            Logger.LogDebug($"Skipping handler: {failures}");
            return HandleRequestResult.SkipHandler();
        }

        var isNumber = long.TryParse(timestamp, out var timestampAsLong);

        if (!isNumber)
        {
            return HandleRequestResult.Fail($"Invalid formatted headers. {TimestampHeaderName} is not a number. ");
        }

        Request.EnableBuffering();
        using var reader = new StreamReader(Request.Body, Encoding.UTF8, false, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        Request.Body.Position = 0;

        if (IsValidSlackSignature(signature, timestampAsLong, body))
        {
            return HandleRequestResult.Success(new AuthenticationTicket(new ClaimsPrincipal(),
                SlackbotEventsAuthenticationConstants.AuthenticationScheme));
        }

        return HandleRequestResult.Fail("Slack request failed signature verification.");
    }

    private bool IsValidSlackSignature(string incomingSignature, long timestamp, string body)
    {
        if (!IsWithinRange(timestamp, TimeSpan.FromMinutes(5)))
        {
            return false;
        }

        return incomingSignature == GeneratedSignature(timestamp, body);
    }

    private static bool IsWithinRange(long timestamp, TimeSpan tolerance)
    {
        return Now - timestamp <= tolerance.TotalSeconds;
    }

    private string GeneratedSignature(long timestamp, string body)
    {
        var signature = $"v0:{timestamp}:{body}";
        var hasher = new HMACSHA256(Encoding.UTF8.GetBytes(_signingSecret));
        var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(signature));
        var builder = new StringBuilder("v0=");
        foreach (var part in hash)
        {
            builder.Append(part.ToString("x2"));
        }

        return builder.ToString();
    }
}
