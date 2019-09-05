using AutoMapper;
using Magalu.Challenge.Data;
using Magalu.Challenge.Web.Api.Models.Product;
using Magalu.Challenge.Web.Api.Services.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : DataController<Product, GetProductModel, SendProductModel>
    {
        private readonly ICustomerAuthorizationService customerAuthorizationService;

        public ProductController(
            IOptions<PaginationOptions> paginationOptions,
            MagaluContext context, 
            IMapper mapper,
            ICustomerAuthorizationService customerAuthorizationService)
            : base(paginationOptions, context, mapper, AllowedActions.Get | AllowedActions.GetPage | AllowedActions.Post | AllowedActions.Put)
        {
            this.customerAuthorizationService = customerAuthorizationService ?? throw new ArgumentNullException(nameof(customerAuthorizationService));
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
            var pageNumber = page.GetValueOrDefault(1);

            var reviews = await
                Context.ProductReviews
                .AsQueryable()
                .Where(r => r.CustomerId == id)
                .SelectPage(pageNumber, DefaultPageSize)
                .ToArrayAsync();

            return Mapper.Map<GetProductReviewModel[]>(reviews);
        }

        [Authorize]
        [HttpPost("{id}/review")]
        public async Task<ActionResult<GetProductReviewModel>> PostReview(long id, [FromBody] SendProductReviewModel model)
        {
            if (!customerAuthorizationService.CustomerIdIsAuthorized(id))
                return Forbid();

            if (!await Context.Products.AnyAsync(p => p.Id == id))
                return NotFound(null);

            if (!await Context.Customers.AnyAsync(c => c.Id == model.CustomerId))
                ModelState.AddModelError(nameof(model.CustomerId), $"Customer with Id {model.CustomerId} does not exist.");

            if (await Context.ProductReviews.AsQueryable().AnyAsync(fp => fp.CustomerId == model.CustomerId && fp.ProductId == id))
                ModelState.AddModelError(nameof(model.CustomerId), $"Customer with id {model.CustomerId} already has a review for product with id {id}.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = Mapper.Map<ProductReview>(model);

            review.ProductId = id;

            await Context.AddAsync(review);
            await Context.SaveChangesAsync();

            return Mapper.Map<GetProductReviewModel>(review);
        }

        [Authorize]
        [HttpPut("{id}/review")]
        public async Task<ActionResult<GetProductReviewModel>> PutReview(long id, [FromBody] SendProductReviewModel model)
        {
            if (!customerAuthorizationService.CustomerIdIsAuthorized(id))
                return Forbid();

            if (!await Context.Products.AnyAsync(p => p.Id == id))
                return NotFound(null);

            if (!await Context.Customers.AnyAsync(c => c.Id == model.CustomerId))
                ModelState.AddModelError(nameof(model.CustomerId), $"Customer with Id {model.CustomerId} does not exist.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = await Context.ProductReviews.SingleOrDefaultAsync(fp => fp.CustomerId == model.CustomerId && fp.ProductId == id);

            if (review == null)
                return NotFound(null);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Mapper.Map(model, review);

            await Context.SaveChangesAsync();

            return Mapper.Map<GetProductReviewModel>(review);
        }

        [Authorize]
        [HttpDelete("{id}/review")]
        public async Task<ActionResult> DeleteReview(long id, [FromBody] DeleteProductReviewModel model)
        {
            if (!customerAuthorizationService.CustomerIdIsAuthorized(id))
                return Forbid();

            if (!await Context.Products.AnyAsync(c => c.Id == id))
                return NotFound(null);

            var review = await Context.ProductReviews.AsQueryable().SingleOrDefaultAsync(fp => fp.CustomerId == model.CustomerId && fp.ProductId == id);

            if (review == null)
                return NotFound(null);

            Context.Remove(review);
            await Context.SaveChangesAsync();

            return Ok(null);
        }
    }
}
