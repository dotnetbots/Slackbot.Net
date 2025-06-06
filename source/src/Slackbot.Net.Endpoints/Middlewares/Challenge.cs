using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Slackbot.Net.Endpoints.Middlewares;

public class Challenge
{
    private readonly ILogger<Challenge> _logger;

    public Challenge(RequestDelegate next, ILogger<Challenge> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var challenge = context.Items[HttpItemKeys.ChallengeKey];

        _logger.LogInformation($"Handling challenge request. Challenge: {challenge}");
        context.Response.StatusCode = 200;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { challenge }));
    }

    public static bool ShouldRun(HttpContext ctx)
    {
        return ctx.Items.ContainsKey(HttpItemKeys.ChallengeKey);
    }
}
