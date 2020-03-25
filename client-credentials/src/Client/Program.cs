using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = new HttpClient();
            var discoveryDocument = client.GetDiscoveryDocumentAsync("http://localhost:5005").GetAwaiter().GetResult();

            if (HasError(discoveryDocument))
            {
                return;
            }

            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            }).GetAwaiter().GetResult();

            if (HasError(tokenResponse))
            {
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            client.SetBearerToken(tokenResponse.AccessToken);

            var response = client.GetAsync("http://localhost:5001/identity").GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Console.WriteLine(JArray.Parse(content));
            }
        }

        static bool HasError (ProtocolResponse response) 
        {
            if (response.IsError)
            {
                Console.WriteLine(response.Error);
            }
            return response.IsError;
        }
    }
}
