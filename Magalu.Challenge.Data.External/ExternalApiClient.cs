using Magalu.Challenge.Data.External.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Magalu.Challenge.Data.External
{
    public sealed class ExternalApiClient : IDisposable
    {
        private readonly HttpClient client;

        private const string baseUrl = "http://challenge-api.luizalabs.com/api";

        public ExternalApiClient()
        {
            client = new HttpClient();
        }

        public async Task<GetProductModel> GetProductAsync(Guid id)
        {
            var url = $"{baseUrl}/product/{id}";
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsAsync<GetProductModel>();
        }

        public GetProductModel GetProduct(Guid id)
        {
            return GetProductAsync(id).Result;
        }

        public void Dispose()
        {
            if (client != null)
                client.Dispose();
        }
    }
}
