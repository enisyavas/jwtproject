using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jwtproject.api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace jwtproject.api
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt=> {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience=true, //token üzerinde Audience doðrulamasýný aktifleþtirdik.
                    ValidateIssuer = true, //token üzerinde Issuer doðrulamasýný aktifleþtirdik.
                    ValidateLifetime =true, //Oluþturulan token deðerinin süresini kontrol edecek olan doðrulamadýr. token deðerinin kullaným süresi doðrulamasýný aktifleþtirdik.
                    ValidateIssuerSigningKey =true, //token deðerinin bu uygulamaya ait olup olmadýðýný anlamamýzý saðlayan Security Key doðrulamasýný aktifleþtirdik.
                    ValidIssuer = Configuration["Token:Issuer"],//Oluþturulacak token deðerini kimin daðýttýðýný ifade edeceðimiz alandýr. Örneðin; “www.myapi.com”
                    ValidAudience = Configuration["Token:Audience"], //Oluþturulacak token deðerini kimlerin/hangi originlerin/sitelerin kullanacaðýný belirlediðimiz alandýr. Örneðin; “www.bilmemne.com”
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"])),//Üretilecek token deðerinin uygulamamýza ait bir deðer olduðunu ifade eden security key verisinin doðrulamasýdýr.
                    ClockSkew =TimeSpan.Zero //Üretilecek token deðerinin expire süresinin belirtildiði deðer kadar uzatýlmasýný saðlayan özelliktir. Örneðin; kullanýlabilirlik süresi 5 dakika olarak ayarlanan token deðerinin ClockSkew deðerine 3 dakika verilirse eðer ilgili token 5 + 3 = 8 dakika kullanýlabilir olacaktýr. Bunun nedeni, aralarýnda zaman farký olan farklý lokasyonlardaki sunucularda yayýn yapan bir uygulama üzerinde elde edilen ortak token deðerinin saati ileride olan sunucuda geçerliliðini daha erken yitirmemesi için ClockSkew propertysi sayesinde aradaki fark kadar zamaný tokena eklememiz gerekmektedir. Böylece kullaným süresi uzatýlmýþ ve tüm sunucularda token deðeri adil kullanýlabilir hale getirilmiþ olunacaktýr.
                };
            });

            services.AddDbContext<DemoContext>(x=>x.UseSqlServer("Data Source=DESKTOP-RK3SKV8;Initial Catalog=JwtProject;Integrated Security=True;"));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
