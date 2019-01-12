using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Logowanie.Data;
using Microsoft.EntityFrameworkCore;

using Logowanie.Models;
using Microsoft.AspNetCore.Identity;
using System.Web;

namespace Logowanie
{
    public class Startup
    {
       
        
        public Startup(IConfiguration configuration)
        {
            

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        


        public void ConfigureServices(IServiceCollection services)
        {
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        


            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

       

            services.AddIdentity<ApplicationUser, IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();
            services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.FromSeconds(10));
            services.AddAuthentication()
                .Services.ConfigureApplicationCookie(options =>
                {
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    

                });
            services.Configure<IdentityOptions>(options =>
            {
                
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromMinutes(10);
               
                
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
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
            .CreateScope())

           
            app.UseSession();


                app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
      
    }
}
