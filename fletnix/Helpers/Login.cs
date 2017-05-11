using System;
using IdentityModel.Client;

namespace fletnix.Helpers
{
    public class Login
    {

        public static async void Auth(string client = "ro.client", string secret = "secret", string username = "alice", string password = "password", string scope = "api1")
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var tokenClient = new TokenClient(disco.TokenEndpoint, client, secret);
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(username, password, scope);

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);

            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

        }
    }
}