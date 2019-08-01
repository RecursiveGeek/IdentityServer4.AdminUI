using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;

namespace IdentityServer4.AdminUI
{
    public class Startup
    {
        private string _serverurl = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _serverurl = Configuration["ConectionStrings:IdentityServer4AdminUIContext"];
            services.AddMvc();

            services.AddDistributedMemoryCache();
            services.AddSession();
            _ = services.Configure<CookiePolicyOptions>(options =>
              {
                  // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                  options.CheckConsentNeeded = context => true;
                  options.MinimumSameSitePolicy = SameSiteMode.None;

              });
            _ = services.AddSession(options =>
              {
                  // Here is where you can set the timeout for the session states.
                  options.IdleTimeout = TimeSpan.FromMinutes(20);
              });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<IdentityServer4AdminUIContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("IdentityServer4AdminUIContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json",
                     optional: false,
                     reloadOnChange: true)
        .AddEnvironmentVariables();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                builder.AddUserSecrets<Startup>();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            _ = app.UseMvc(routes =>
              {
                  routes.MapRoute(
                      name: "default",
                      template: "{controller=Clients}/{action=Index}/{id?}"
                      );

                  routes.MapRoute(
                        "NotFound",
                         "{*url}",
                          new { controller = "Clients", action = "PageNotFound" }
                          );
              });
        }
    }
}
