using Microsoft.AspNetCore.Session;
using VSW.Core;
using VSW.Core.Services;
using VSW.Website.Extensions;
using VSW.Website.Middleware;

namespace VSW.Website
{
    public class WebStartUp : StartUpBase<IApplicationBuilder, IWebHostEnvironment>
    {
        public WebStartUp(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            //Config Cookies
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var appSetting = Configuration.GetConfigWithSection<AppsSetting>();
            var mvcBuilder = services.AddControllersWithViews()
                                    .AddSessionStateTempDataProvider();

            mvcBuilder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddHttpContextAccessor();
            services.AddSession(options =>
            {
                options.Cookie.CustomSessionOptions(appSetting);

                //Nếu 1 tiếng không có request truy cập thì Session sẽ hết hạn
                options.IdleTimeout = TimeSpan.FromHours(1);
            });

            services.AddTransient<DistributedSessionStore>();
            services.AddTransient<ISessionStore, CustomDistributedSessionStore>();

            services.AddLocalization();
            //Xóa X-Frame-Options
            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
        }

        protected override void OnConfigure(IApplicationBuilder app)
        {
            CoreService.ServiceProvider = app.ApplicationServices;

            var appSetting = Configuration.GetConfigWithSection<AppsSetting>();
            if (Environments.IsDevelopment() || appSetting.Debug)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                if (appSetting.MultiSite)
                {
                    app.UseExceptionHandler("/vn/Home/Error");
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            try
            {
                app.UseTracing(CoreService.Configuration);
            }
            catch (Exception ex)
            {
                GlobalLogger.Current.LogError(ex, "Use Tracing is error");
            }
            if (appSetting.MultiSite)
            {
                app.UseStatusCodePagesWithReExecute("/vn/Home/Error");
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            //app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseWebAuthorization();
            app.UseRequestLocalization();
            app.UseMiddleware<DynamicRouteMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                if (appSetting.MultiSite)
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{site}/{controller}/{action}/{id?}");
                }
                else
                {
                    endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
                }
            });
        }
    }
}
