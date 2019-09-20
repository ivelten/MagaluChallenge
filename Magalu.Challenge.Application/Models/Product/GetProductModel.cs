using System;

namespace Magalu.Challenge.Application.Models.Product
{
    public class GetProductModel
    {
        public Guid Id { get; set; }

        public string Brand { get; set; }

        public string Image { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public float? ReviewScore { get; set; }
    }
}
