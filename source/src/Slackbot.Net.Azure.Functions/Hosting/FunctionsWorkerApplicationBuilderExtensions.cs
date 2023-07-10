using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Slackbot.Net.Azure.Functions.Middlewares;

namespace Slackbot.Net.Azure.Functions.Hosting;

public static class FunctionsWorkerApplicationBuilderExtensions
{
    public static IFunctionsWorkerApplicationBuilder UseSlackbot(this IFunctionsWorkerApplicationBuilder app, bool enableAuth = true)
    {
        if (enableAuth)
            app.UseMiddleware<SlackbotEventAuthMiddleware>();

        app.UseMiddleware<HttpItemsManager>();
        app.UseMiddleware<Challenge>();
        app.UseMiddleware<AppMentionEvents>();
        // app.UseWhen(Uninstall.ShouldRun, b => b.UseMiddleware<Uninstall>());
        // app.UseWhen(MemberJoinedEvents.ShouldRun, b => b.UseMiddleware<MemberJoinedEvents>());
        // app.UseWhen(AppHomeOpenedEvents.ShouldRun, b => b.UseMiddleware<AppHomeOpenedEvents>());
        // app.UseWhen(InteractiveEvents.ShouldRun, b => b.UseMiddleware<InteractiveEvents>());

        return app;
    }
}
