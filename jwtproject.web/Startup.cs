using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jwtproject.web.ApiService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace jwtproject.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddHttpClient<TestApiService>(options =>
            {
                options.BaseAddress = new Uri(Configuration["baseUrl"]);
            });

            services.AddHttpContextAccessor();

            services.AddAuthentication(options => {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "jwtCookie";
                options.LoginPath = new PathString("/Home/SignIn");
                options.LogoutPath = new PathString("/Home/SignOut");
                options.ExpireTimeSpan = TimeSpan.FromDays(60);
                options.AccessDeniedPath = new PathString("/Home/AccessDenied");
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

            });
            

            //CookieBuilder cookieBuilder = new CookieBuilder();
            //cookieBuilder.Name = "jwtCookie";
            //cookieBuilder.HttpOnly = true;
            ////cookieBuilder.Expiration = TimeSpan.FromDays(60);
            //cookieBuilder.SameSite = SameSiteMode.Lax;
            //cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //
            //services.ConfigureApplicationCookie(options => {
            //    options.LoginPath = new PathString("/Home/Login");
            //    options.LogoutPath = new PathString("/Home/SignOut");
            //    options.ExpireTimeSpan = TimeSpan.FromDays(60);
            //    options.Cookie = cookieBuilder;
            //    options.SlidingExpiration = true;
            //    options.AccessDeniedPath = new PathString("/Home/AccessDenied");
            //});

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
