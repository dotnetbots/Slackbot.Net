using Microsoft.AspNetCore.Http;
using Slackbot.Net.Endpoints.Interactive;
using Slackbot.Net.Endpoints.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    public static class IAppBuilderExtensions
    {
        public static IApplicationBuilder UseInteractiveSlackbotEndpoints(this IApplicationBuilder app, string path)
        {
            return app.MapWhen(c => c.Request.Method == "POST" && c.Request.Path == path,
                a => a.UseMiddleware<InteractiveEventsMiddleware>());
        }

        public static IApplicationBuilder UseSlackbotEvents(this IApplicationBuilder app, string path = "/events")
        {
            app.MapWhen(c => IsSlackRequest(c, path), a =>
            {
                a.UseMiddleware<HttpItemsManager>();
                a.MapWhen(Challenge.ShouldRun, b => b.UseMiddleware<Challenge>());
                a.MapWhen(Uninstall.ShouldRun, b => b.UseMiddleware<Uninstall>());
                a.MapWhen(Events.ShouldRun, b => b.UseMiddleware<Events>());            
            });
   
            return app;
        }

        private static bool IsSlackRequest(HttpContext ctx, string path)
        {
            return ctx.Request.Path == path && ctx.Request.Method == "POST" && ctx.Request.ContentType == "application/json";
        }
    }
}