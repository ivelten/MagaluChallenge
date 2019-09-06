using Magalu.Challenge.Application.Models.Product;
using Magalu.Challenge.ApplicationServices;
using Magalu.Challenge.Domain;
using Magalu.Challenge.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : DataController<Product, GetProductModel, SendProductModel>
    {
        protected readonly IProductReviewService ProductReviewService;

        public ProductController(
            IDataService<Product, GetProductModel, SendProductModel> dataService,
            IProductReviewService productReviewService)
            : base(dataService, AllowedActions.Get | AllowedActions.GetPage | AllowedActions.Post | AllowedActions.Put)
        {
            ProductReviewService = productReviewService ?? throw new ArgumentNullException(nameof(productReviewService));
        }

        [Authorize(Roles = Roles.Administrator)]
        public override Task<ActionResult<GetProductModel>> Post([FromBody] SendProductModel model)
        {
            return base.Post(model);
        }

        [Authorize(Roles = Roles.Administrator)]
        public override Task<ActionResult<GetProductModel>> Put(long id, [FromBody] SendProductModel model)
        {
            return base.Put(id, model);
        }

        [HttpGet("{id}/review")]
        public async Task<ActionResult<IEnumerable<GetProductReviewModel>>> GetReviews(long id, int? page)
        {
            return GetResult(await ProductReviewService.GetProductReviewsOfProductAsync(id, page));
        }
    }
}
