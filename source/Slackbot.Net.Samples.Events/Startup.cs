using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Slackbot.Net.Endpoints.Hosting;
using Slackbot.Net.Samples.Distributed;

namespace Slackbot.Net.Samples.Events
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSlackBotEvents<MyTokenStore>()
                .AddHandler<AppMentionHandler>()
                .AddHandler<OtherAppMentionHandler>()
                .AddShortcut<Shortcutter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSlackbotEvents();
        }
    }
}