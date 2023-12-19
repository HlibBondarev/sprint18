using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskAuthenticationAuthorization.Models;
using TaskAuthenticationAuthorization.Restrictions;

namespace TaskAuthenticationAuthorization
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ShoppingContext>(options => options.UseSqlServer(connection));
            services.AddControllersWithViews();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options => //CookieAuthenticationOptions
				{
					options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
				});

			services.AddTransient<IAuthorizationHandler, BuyerTypeHandler>();

            services.AddAuthorization(opts =>
            {
                //services.AddAuthorization(options =>
                //{
                //    options.AddPolicy("OnlyForUser", policy =>
                //          policy.RequireClaim("Email"));
                //});

                //opts.AddPolicy("OnlyForAdmin", policy =>
                //{
                //    policy.RequireRole("admin");
                //});

                //opts.AddPolicy("OnlyForBuyerType", policy => {
                //    policy.RequireClaim("TypeOfByer", Enum.GetNames(typeof(BuyerType)));
                //});

                //opts.AddPolicy("OnlyForBuyerType_are_Golden_and_Wholesale",
                //    policy => policy.Requirements
                //                    .Add(new BuyerTypeRequirement
                //                    (
                //                         new List<BuyerType> { BuyerType.Golden, BuyerType.Wholesale }))
                //                    );

                opts.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimsIdentity.DefaultRoleClaimType, "admin"));
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
