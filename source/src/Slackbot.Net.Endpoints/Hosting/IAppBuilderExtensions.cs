using Slackbot.Net.Endpoints.Hosting;

namespace Microsoft.AspNetCore.Builder
{
    public static class IAppBuilderExtensions
    {
        public static IApplicationBuilder UseSlackbotEndpoint(this IApplicationBuilder app, string path)
        {
            return app.MapWhen(c => c.Request.Method == "POST" && c.Request.Path == path,
                a => a.UseMiddleware<HttpResponderMiddleware>());
        }
    }
}