using System;

namespace Magalu.Challenge.Application.Models.Customer
{
    public class GetFavoriteProductModel
    {
        public Guid ProductId { get; set; }

        public Guid CustomerId { get; set; }
    }
}
