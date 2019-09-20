using Magalu.Challenge.Data.External.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Magalu.Challenge.Data.External
{
    public interface IExternalProductApiClient : IDisposable
    {
        Task<GetProductModel> GetAsync(Guid id);

        Task<IList<GetProductModel>> GetPageAsync(int pageNumber);
    }

    public class ExternalProductApiClient : IExternalProductApiClient
    {
        private readonly HttpClient client;

        private const string baseUrl = "http://challenge-api.luizalabs.com/api";

        public ExternalProductApiClient()
        {
            client = new HttpClient();
        }

        public async Task<GetProductModel> GetAsync(Guid id)
        {
            var url = $"{baseUrl}/product/{id}";
            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<GetProductModel>();
        }

        public async Task<IList<GetProductModel>> GetPageAsync(int pageNumber)
        {
            var url = $"{baseUrl}/product/?page={pageNumber}";
            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return new List<GetProductModel>();

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsAsync<GetProductPageModel>();

            return content.Products;
        }

        public void Dispose()
        {
            if (client != null)
                client.Dispose();
        }
    }
}
