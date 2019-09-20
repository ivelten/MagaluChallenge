using Magalu.Challenge.Data.External.Models;
using Magalu.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Magalu.Challenge.Data.External
{
    public class ExternalProductApiRepository : IReadOnlyRepository<Product>
    {
        protected readonly IExternalProductApiClient Client;
        protected const int ApiPageSize = 100;

        public ExternalProductApiRepository(
            IExternalProductApiClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        private static Product ConvertToProduct(GetProductModel model)
        {
            return new Product
            {
                Id = model.Id,
                Brand = model.Brand,
                Image = model.Image,
                Price = model.Price,
                Title = model.Title
            };
        }

        public async Task<Product> FindAsync(params object[] keyValues)
        {
            if (keyValues == null)
                throw new ArgumentNullException(nameof(keyValues));

            if (keyValues.Length != 1)
                throw new ArgumentException("Key values must have only one item for this repository.", nameof(keyValues));

            if (keyValues[0] is Guid casted)
                return ConvertToProduct(await Client.GetAsync(casted));

            throw new ArgumentException("Kay values single item must be a Guid.", nameof(keyValues));
        }

        public async Task<IPagedList<Product>> GetPagedListAsync(int pageIndex)
        {
            var result = await Client.GetPageAsync(pageIndex + 1);

            if (result == null)
                return null;

            return new ExternalProductApiPagedList(Client, result.Select(ConvertToProduct).ToList(), pageIndex);
        }
    }
}
