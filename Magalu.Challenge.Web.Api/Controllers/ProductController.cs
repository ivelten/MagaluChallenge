using Magalu.Challenge.Application.Models.Product;
using Magalu.Challenge.ApplicationServices;
using Magalu.Challenge.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ReadOnlyDataController<Product, GetProductModel>
    {
        protected readonly IProductReviewService ProductReviewService;

        public ProductController(
            IReadOnlyDataService<Product, GetProductModel> dataService,
            IProductReviewService productReviewService)
            : base(dataService)
        {
            ProductReviewService = productReviewService ?? throw new ArgumentNullException(nameof(productReviewService));
        }

        [HttpGet("{id}/review")]
        public async Task<ActionResult<IEnumerable<GetProductReviewModel>>> GetReviews(Guid id, int? page)
        {
            return GetResult(await ProductReviewService.GetProductReviewsOfProductAsync(id, page));
        }
    }
}
