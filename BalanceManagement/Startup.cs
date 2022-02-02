using BalanceManagement.AuthConfig;
using BalanceManagement.Injection.Auth;
using BalanceManagement.Injection.IdentityServer;
using BalanceManagement.Injection.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalanceManagement
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
            string apiHost = Configuration.GetValue<string>("APIHost").TrimEnd('/') + "/";
            #region Identity Server
            services.IdentityServerInjection(apiHost);
            #endregion
            #region Auth Configuration
            services.AddAuthConfiguration(apiHost);
            #endregion
            services.AddControllers();
            #region Swagger Docs
            services.AddSwaggerDocumentation(apiHost);
            #endregion
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseIdentityServer();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            #region Use Swagger
            app.UseSwaggerDocumentation();
            #endregion
        }
    }
}
