using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace BalanceManagement.Injection.Auth
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddAuthConfiguration(this IServiceCollection services, string apiHost)
        {
            services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                options.TokenValidationParameters.RequireExpirationTime = false;
                options.Authority = apiHost;
                options.RequireHttpsMetadata = false;
                options.Audience = "Api";
                options.BackchannelHttpHandler = new HttpClientHandler
                {
                    UseProxy = false
                };
            });
            services.AddAuthorization(c =>
            {
                c.AddPolicy("BalanceManagementApi", p =>
                {
                    p.RequireClaim("scope", new string[]
                    {
                        "BalanceManagementApi"
                    });
                });
            });
            return services;
        }
    }
}
