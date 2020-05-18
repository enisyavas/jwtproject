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
                    ValidateAudience=true, //token �zerinde Audience do�rulamas�n� aktifle�tirdik.
                    ValidateIssuer = true, //token �zerinde Issuer do�rulamas�n� aktifle�tirdik.
                    ValidateLifetime =true, //Olu�turulan token de�erinin s�resini kontrol edecek olan do�rulamad�r. token de�erinin kullan�m s�resi do�rulamas�n� aktifle�tirdik.
                    ValidateIssuerSigningKey =true, //token de�erinin bu uygulamaya ait olup olmad���n� anlamam�z� sa�layan Security Key do�rulamas�n� aktifle�tirdik.
                    ValidIssuer = Configuration["Token:Issuer"],//Olu�turulacak token de�erini kimin da��tt���n� ifade edece�imiz aland�r. �rne�in; �www.myapi.com�
                    ValidAudience = Configuration["Token:Audience"], //Olu�turulacak token de�erini kimlerin/hangi originlerin/sitelerin kullanaca��n� belirledi�imiz aland�r. �rne�in; �www.bilmemne.com�
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"])),//�retilecek token de�erinin uygulamam�za ait bir de�er oldu�unu ifade eden security key verisinin do�rulamas�d�r.
                    ClockSkew =TimeSpan.Zero //�retilecek token de�erinin expire s�resinin belirtildi�i de�er kadar uzat�lmas�n� sa�layan �zelliktir. �rne�in; kullan�labilirlik s�resi 5 dakika olarak ayarlanan token de�erinin ClockSkew de�erine 3 dakika verilirse e�er ilgili token 5 + 3 = 8 dakika kullan�labilir olacakt�r. Bunun nedeni, aralar�nda zaman fark� olan farkl� lokasyonlardaki sunucularda yay�n yapan bir uygulama �zerinde elde edilen ortak token de�erinin saati ileride olan sunucuda ge�erlili�ini daha erken yitirmemesi i�in ClockSkew propertysi sayesinde aradaki fark kadar zaman� tokena eklememiz gerekmektedir. B�ylece kullan�m s�resi uzat�lm�� ve t�m sunucularda token de�eri adil kullan�labilir hale getirilmi� olunacakt�r.
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
