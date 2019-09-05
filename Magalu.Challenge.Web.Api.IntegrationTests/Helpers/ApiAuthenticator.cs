using Magalu.Challenge.Web.Api.Models.User;
using System;
using System.Net.Http;
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
    }
}
