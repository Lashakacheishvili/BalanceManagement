using IdentityServer4.Models;
using System.Collections.Generic;

namespace BalanceManagement.AuthConfig
{
    public class ClientConfiguration
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            yield return new ApiResource
            {
                Name = "Api",
                DisplayName = "BalanceManagement Api",
                Scopes = new[]
                {
                  "BalanceManagementApi"
                }
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "BalanceManagement",
                    AllowedGrantTypes = GrantTypes.ClientCredentials ,
                    AllowOfflineAccess=true,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes = new[] { "BalanceManagementApi" },
                    ClientSecrets = new[] { new Secret("5Aue2ks34fj".Sha256()) },
                    AccessTokenLifetime = 3600*24*365*10
                }
            };
        }
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("BalanceManagementApi")
            };
        }
    }
}
