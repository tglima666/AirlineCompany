using AirlineCompany.web.Data;
using AirlineCompany.web.Data.Entities;
using AirlineCompany.web.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web
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
            //Configurar a autenticação
            services.AddIdentity<User, IdentityRole>(ac =>
            {
                ac.User.RequireUniqueEmail = true;
                ac.Password.RequireDigit = false;
                ac.Password.RequiredUniqueChars = 0;
                ac.Password.RequireLowercase = false;
                ac.Password.RequireUppercase = false;
                ac.Password.RequireNonAlphanumeric = false;
                ac.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<DataContext>();

            //Vamos usar um serviço de SQLServer
            services.AddDbContext<DataContext>(ac =>
            {
                ac.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            //Vou usar a minha classe SeedDb para alimentar as tabelas da BD
            services.AddTransient<SeedDb>();

            services.AddScoped<IRepository, Repository>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
