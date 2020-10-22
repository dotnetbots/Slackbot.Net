using CronBackgroundServices.Extensions.Samples.HelloWorld;
using CronBackgroundServices.Samples.Distributed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Slackbot.Net.Endpoints.Hosting;
using Slackbot.Net.SlackClients.Http.Extensions;

namespace CronBackgroundServices.Samples.Events
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddSlackClientBuilder();
            services.AddRecurringActions().AddRecurrer<WorkspacesAction>().AddRecurrer<HelloWorldRecurrer>().Build();
            
            services.AddSlackBotEvents<MyTokenStore>()
                .AddAppMentionHandler<AppMentionHandler>()
                .AddAppMentionHandler<OtherAppMentionHandler>()
                .AddAppMentionHandler<MemberJoinedChannelHandler>()
                .AddShortcut<Shortcutter>()
                .AddViewSubmissionHandler<AppHomeViewSubmissionHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSlackbot();
        }
    }
}