using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityServer.Helpers
{
    internal class Clients {
        private static IConfigurationRoot _Configuration;

        public Clients(IConfigurationRoot Configuration){
            _Configuration = Configuration;
        }
        public static IEnumerable<Client> Get() {
			//var redirectUri = "http://localhost:5002";
			var redirectUri = "https://fletnix.azurewebsites.net";
            return new List<Client> {
                new Client {
                    ClientId = "fletnix",
                    ClientSecrets = new List<Secret>{new Secret("secret".Sha256())},
                    ClientName = "Fletnix totally not a copy of netflix...",
					AllowedGrantTypes = GrantTypes.List(
                        GrantType.Implicit,
						GrantType.ClientCredentials),
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role"
                    },
                    RedirectUris = new List<string> {redirectUri +"/signin-oidc"},
                    PostLogoutRedirectUris = new List<string> {redirectUri }
                }
            };
        }
    }
}