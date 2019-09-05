using System.Threading.Tasks;
using AutoMapper;
using Magalu.Challenge.Data;
using Magalu.Challenge.Web.Api.Models.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Magalu.Challenge.Web.Api.Models.Product;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Magalu.Challenge.Web.Api.Services.Authorization;
using System;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : DataController<Customer, GetCustomerModel, SendCustomerModel>
    {
        private readonly ICustomerAuthorizationService customerAuthorizationService;

        public CustomerController(
            IOptions<PaginationOptions> paginationOptions,
            MagaluContext context,
            IMapper mapper,
            ICustomerAuthorizationService customerAuthorizationService)
            : base(paginationOptions, context, mapper, AllowedActions.All)
        {
            this.customerAuthorizationService = customerAuthorizationService ?? throw new ArgumentNullException(nameof(customerAuthorizationService));
        }

        [Authorize(Roles = Roles.Administrator)]
        public override async Task<ActionResult<GetCustomerModel>> Post(SendCustomerModel model)
        {
            if (await Context.Customers.AnyAsync(c => c.Email == model.Email))
                ModelState.AddModelError(nameof(model.Email), $"E-mail address '{model.Email}' is already being used by another customer.");

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
            var pageNumber = page.GetValueOrDefault(1);

            var products = await
                Context.FavoriteProducts
                .AsQueryable()
                .Where(fp => fp.CustomerId == id)
                .Select(fp => fp.Product)
                .SelectPage(pageNumber, DefaultPageSize)
                .ToArrayAsync();

            return Mapper.Map<GetProductModel[]>(products);
        }

        [Authorize]
        [HttpPost("{id}/favorite_product")]
        public async Task<ActionResult<GetFavoriteProductModel>> PostFavoriteProduct(long id, [FromBody] SendFavoriteProductModel model)
        {
            if (!customerAuthorizationService.CustomerIdIsAuthorized(id))
                return Forbid();

            if (!await Context.Products.AnyAsync(p => p.Id == model.ProductId))
                ModelState.AddModelError(nameof(model.ProductId), $"Product with Id {model.ProductId} does not exist.");

            if (await Context.FavoriteProducts.AsQueryable().AnyAsync(fp => fp.ProductId == model.ProductId && fp.CustomerId == id))
                ModelState.AddModelError(nameof(model.ProductId), $"Product with Id {model.ProductId} is already a favorite product of customer with Id {id}.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var favorite = Mapper.Map<FavoriteProduct>(model);

            favorite.CustomerId = id;

            await Context.AddAsync(favorite);
            await Context.SaveChangesAsync();

            return Mapper.Map<GetFavoriteProductModel>(favorite);
        }

        [Authorize]
        [HttpDelete("{id}/favorite_product")]
        public async Task<ActionResult> DeleteFavoriteProduct(long id, [FromBody] DeleteFavoriteProductModel model)
        {
            if (!customerAuthorizationService.CustomerIdIsAuthorized(id))
                return Forbid();

            if (!await Context.Customers.AnyAsync(c => c.Id == id))
                return NotFound(null);

            var relationship = await Context.FavoriteProducts.AsQueryable().SingleOrDefaultAsync(fp => fp.ProductId == model.ProductId && fp.CustomerId == id);

            if (relationship == null)
                return NotFound(null);

            Context.Remove(relationship);
            await Context.SaveChangesAsync();

            return Ok(null);
        }
    }
}
