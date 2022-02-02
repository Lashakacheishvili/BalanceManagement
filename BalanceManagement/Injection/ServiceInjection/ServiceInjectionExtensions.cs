using Balances;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

namespace BalanceManagement.Injection.ServiceInjection
{
    public static class ServiceInjectionExtensions
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped<IBalanceManager, CasinoBalanceManager>();
            services.AddScoped<IBalanceManager, GameBalanceManager>();
            return services;
        }
    }
}
