using Slackbot.Net.Endpoints.Generic;
using Slackbot.Net.Endpoints.Hosting;
using Slackbot.Net.Endpoints.Interactive;

namespace Microsoft.AspNetCore.Builder
{
    public static class IAppBuilderExtensions
    {
        public static IApplicationBuilder UseSlackbotEndpoint(this IApplicationBuilder app, string path)
        {
            return app.MapWhen(c => c.Request.Method == "POST" && c.Request.Path == path,
                a => a.UseMiddleware<GenericeEventsMiddleware>());
        }

        public static IApplicationBuilder UseSlackbotEvents(this IApplicationBuilder app, string path)
        {
            return app.MapWhen(c => c.Request.Method == "POST" && c.Request.Path == path,
                a => a.UseMiddleware<SlackEventsMiddleware>());
        }
    }
}