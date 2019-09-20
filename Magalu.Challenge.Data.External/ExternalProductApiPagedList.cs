using Magalu.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Magalu.Challenge.Data.External
{
    public sealed class ExternalProductApiPagedList : IPagedList<Product>
    {
        private readonly IExternalProductApiClient client;

        internal ExternalProductApiPagedList(IExternalProductApiClient client, IList<Product> items, int pageIndex)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            Items = items ?? throw new ArgumentNullException(nameof(items));
            PageIndex = pageIndex;
        }

        public int IndexFrom => 0;

        public int PageIndex { get; private set; }

        public int PageSize => Constants.PageSize;

        public int TotalCount
        {
            get
            {
                var count = Items.Count;

                for (int page = PageIndex + 2; ; page++)
                {
                    var result = client.GetPageAsync(page).Result;

                    if (result == null || result.Count == 0)
                        break;

                    count += result.Count;
                }

                for (int page = PageIndex; page >= 1; page--)
                {
                    var result = client.GetPageAsync(page).Result;

                    if (result == null || result.Count == 0)
                        continue;

                    count += result.Count;
                }

                return count;
            }
        }

        public int TotalPages
        {
            get
            {
                var count = 1;

                for (int page = PageIndex + 2; ; page++)
                {
                    var result = client.GetPageAsync(page).Result;

                    if (result == null || result.Count == 0)
                        break;

                    count++;
                }

                for (int page = PageIndex; page >= 1; page--)
                {
                    var result = client.GetPageAsync(page).Result;

                    if (result == null || result.Count == 0)
                        continue;

                    count++;
                }

                return count;
            }
        }

        public IList<Product> Items { get; private set; }

        public bool HasPreviousPage
        {
            get
            {
                var result = client.GetPageAsync(PageIndex).Result;

                return (result != null && result.Count > 0);
            }
        }

        public bool HasNextPage
        {
            get
            {
                var result = client.GetPageAsync(PageIndex + 2).Result;

                return (result != null && result.Count > 0);
            }
        }
    }
}
