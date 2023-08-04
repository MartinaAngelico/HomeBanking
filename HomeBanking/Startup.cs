using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeBanking
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
            services.AddRazorPages();
            services.AddControllers().AddJsonOptions(x =>x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            //agregamos el contexto de la base de datos
            services.AddDbContext<HomeBankingContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("HomeBankingConexion")));

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            //autenticacion
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                     options.ExpireTimeSpan = TimeSpan.FromMinutes(10); //tiempo de la expiracion de la cookie
                     options.LoginPath = new PathString("/index.html"); //indica que ruta debe redirigir el servidor cuando no existe una sesion
                 });

            //autorización
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ClientOnly", policy => policy.RequireClaim("Client"));
            });
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            //le decimos que use autenticacion
            app.UseAuthentication();
            //autorizacion
            app.UseAuthorization();

            app.UseEndpoints(endpoints => //puntos de acceso de la aplicacion
            {
            endpoints.MapRazorPages(); //Razor es una tecnologia para crear paginas web con C# con HTML
            endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=games}/{ action = Get}"); //aGREGAMOS ESTA LINEA PARA QUE SE PUEDAN USAR LOS CONTROLADORES
            });

        }
    }
}
