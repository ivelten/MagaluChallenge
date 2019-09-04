using System.Threading.Tasks;
using AutoMapper;
using Magalu.Challenge.Data;
using Magalu.Challenge.Web.Api.Models.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Magalu.Challenge.Web.Api.Models.Product;
using System.Collections.Generic;
using System.Linq;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : DataController<Customer, GetCustomerModel, SendCustomerModel>
    {
        public CustomerController(IConfiguration configuration, MagaluContext context, IMapper mapper)
            : base(configuration, context, mapper, AllowedActions.All)
        {
        }

        public override async Task<ActionResult<GetCustomerModel>> Post(SendCustomerModel model)
        {
            if (await Context.Customers.AnyAsync(c => c.Email == model.Email))
                ModelState.AddModelError(nameof(model.Email), $"E-mail address '{model.Email}' is already being used by another customer.");

            return await base.Post(model);
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

        [HttpPost("{id}/favorite_product")]
        public async Task<ActionResult<GetFavoriteProductModel>> PostFavoriteProduct(long id, [FromBody] SendFavoriteProductModel model)
        {
            if (!await Context.Customers.AnyAsync(c => c.Id == id))
                return NotFound(null);

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

        [HttpDelete("{id}/favorite_product")]
        public async Task<ActionResult> DeleteFavoriteProduct(long id, [FromBody] DeleteFavoriteProductModel model)
        {
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
