using BalanceManagement.Injection.Auth;
using BalanceManagement.Injection.IdentityServer;
using BalanceManagement.Injection.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.InjectionService;

namespace BalanceManagement
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
            string apiHost = Configuration.GetValue<string>("APIHost").TrimEnd('/') + "/";
            services.IdentityServerInjection(apiHost);
            services.AddAuthConfiguration(apiHost);
            services.AddControllers();
            services.AddSwaggerDocumentation(apiHost);
            services.AddService();
        }

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
