using System;

namespace Magalu.Challenge.Data.External.Models
{
    public class GetProductModel
    {
        public Guid Id { get; set; }

        public decimal Price { get; set; }

        public string Brand { get; set; }

        public string Image { get; set; }

        public string Title { get; set; }
    }
}
