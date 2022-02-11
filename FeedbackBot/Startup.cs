using FeedbackBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TGUI.CoreLib;
using TGUI.CoreLib.Interfaces;

namespace FeedbackBot
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            TGUIStarter.Init(services);
            services.AddSingleton<IBotCore, FeedbackBotCore>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
            });
        }
    }
}
