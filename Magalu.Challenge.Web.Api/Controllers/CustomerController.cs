using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;
using Magalu.Challenge.Domain;
using Magalu.Challenge.Domain.Entities;
using Magalu.Challenge.Application.Models.Customer;
using Magalu.Challenge.ApplicationServices;
using Magalu.Challenge.Application.Models.Product;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : DataController<Customer, GetCustomerModel, SendCustomerModel>
    {
        protected readonly IFavoriteProductService FavoriteProductService;

        protected readonly IProductReviewService ProductReviewService;

        public CustomerController(
            IDataService<Customer, GetCustomerModel, SendCustomerModel> dataService,
            IFavoriteProductService favoriteProductService,
            IProductReviewService productReviewService)
            : base(dataService, AllowedActions.All)
        {
            FavoriteProductService = favoriteProductService ?? throw new ArgumentNullException(nameof(favoriteProductService));
            ProductReviewService = productReviewService ?? throw new ArgumentNullException(nameof(productReviewService));
        }

        [Authorize(Roles = Roles.Administrator)]
        public override async Task<ActionResult<GetCustomerModel>> Post(SendCustomerModel model)
        {
            return await base.Post(model);
        }

        [Authorize(Roles = Roles.Administrator)]
        public override Task<ActionResult<GetCustomerModel>> Put(long id, [FromBody] SendCustomerModel model)
        {
            return base.Put(id, model);
        }

        [Authorize(Roles = Roles.Administrator)]
        public override Task<ActionResult> Delete(long id)
        {
            return base.Delete(id);
        }

        [HttpGet("{id}/favorite_product")]
        public async Task<ActionResult<IEnumerable<GetProductModel>>> GetFavoriteProducts(long id, int? page)
        {
            return GetResult(await FavoriteProductService.GetFavoriteProductsAsync(id, page));
        }

        [Authorize]
        [HttpPost("{id}/favorite_product")]
        public async Task<ActionResult<GetFavoriteProductModel>> PostFavoriteProduct(long id, [FromBody] SendFavoriteProductModel model)
        {
            return GetResult(await FavoriteProductService.SaveFavoriteProductAsync(id, model));
        }

        [Authorize]
        [HttpDelete("{id}/favorite_product")]
        public async Task<ActionResult> DeleteFavoriteProduct(long id, [FromBody] DeleteFavoriteProductModel model)
        {
            return GetResult(await FavoriteProductService.DeleteFavoriteProduct(id, model));
        }

        [HttpGet("{id}/product_review")]
        public async Task<ActionResult<IEnumerable<GetProductReviewModel>>> GetProductReviews(long id, int? page)
        {
            return GetResult(await ProductReviewService.GetProductReviewsOfCustomerAsync(id, page));
        }

        [Authorize]
        [HttpPut("{id}/product_review")]
        public async Task<ActionResult<GetProductReviewModel>> PutReview(long id, [FromBody] SendProductReviewModel model)
        {
            return GetResult(await ProductReviewService.UpdateReviewAsync(id, model));
        }

        [Authorize]
        [HttpPost("{id}/product_review")]
        public async Task<ActionResult<GetProductReviewModel>> PostReview(long id, [FromBody] SendProductReviewModel model)
        {
            return GetResult(await ProductReviewService.SaveReviewAsync(id, model));
        }

        [Authorize]
        [HttpDelete("{id}/product_review")]
        public async Task<ActionResult> DeleteReview(long id, [FromBody] DeleteProductReviewModel model)
        {
            return GetResult(await ProductReviewService.DeleteReviewAsync(id, model));
        }
    }
}
