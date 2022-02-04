using Balances;
using Microsoft.Extensions.DependencyInjection;
using Service.ServiceImplementations;
using Service.ServiceInterfaces;

namespace Service.InjectionService
{
    public static class ServiceInjectionExtensions
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped<IBaseManagementService, GameManagementService>();
            return services;
        }
    }
}
