using BalanceManagement.AuthConfig;
using Microsoft.Extensions.DependencyInjection;

namespace BalanceManagement.Injection.IdentityServer
{
    public static class IdentityServerExtensions
    {
        public static IServiceCollection IdentityServerInjection(this IServiceCollection services, string apiHost)
        {
            services.AddIdentityServer(a =>
            {
                a.IssuerUri = apiHost;
            })
            .AddDeveloperSigningCredential()
            .AddInMemoryPersistedGrants()
            .AddInMemoryApiResources(ClientConfiguration.GetApiResources())
            .AddInMemoryClients(ClientConfiguration.GetClients())
            .AddInMemoryApiScopes(ClientConfiguration.GetApiScopes());
            return services;
        }
    }
}
