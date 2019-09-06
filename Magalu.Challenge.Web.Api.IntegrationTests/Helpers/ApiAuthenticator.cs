using Magalu.Challenge.Application.Models.User;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public class ApiAuthenticator
    {
        private readonly HttpClient client;

        public ApiAuthenticator(HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<AuthenticatedUserModel> AuthenticateAsync(string username, string password)
        {
            var request = new AuthenticateUserModel
            {
                Username = username,
                Password = password
            };

            var response = await client.PostAsJsonAsync("/api/user/authenticate", request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsAsync<AuthenticatedUserModel>();
        }

        public async Task AuthenticateClientAsync(HttpClient client, string username, string password)
        {
            var authResult = await AuthenticateAsync(username, password);

            if (authResult == null)
                throw new InvalidOperationException($"Authentication for username '{username}' has failed.");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.JwtToken);
        }
    }
}
