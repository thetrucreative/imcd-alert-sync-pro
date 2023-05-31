using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace imcd_alert_sync_pro.Models
{
    public class StartupModel
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //add services if needed
            //services.AddMvc();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>(); 
            services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>(); 
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //configure app
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    //areaName: "C:/imcd-alert-sync-pro/imcd-alert-sync-pro/Views/Alerts",
                    pattern: "{controller=Alerts}/{action=index}/{id?}");
            });
        }
    }
}
