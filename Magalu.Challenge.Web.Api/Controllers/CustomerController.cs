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
                ModelState.AddModelError("Email", $"E-mail address '{model.Email}' is already being used by another customer.");

            return await base.Post(model);
        }

        [HttpGet("{id}/favorite_products")]
        public async Task<ActionResult<IEnumerable<GetProductModel>>> GetFavoriteProducts(long id, int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            var products = await
                Context.CustomerFavoriteProducts
                .AsQueryable()
                .Where(fp => fp.CustomerId == id)
                .Select(fp => fp.Product)
                .SelectPage(pageNumber, DefaultPageSize)
                .ToArrayAsync();

            return Mapper.Map<GetProductModel[]>(products);
        }

        [HttpPost("{id}/favorite_products")]
        public async Task<ActionResult<GetFavoriteProductModel>> AddFavoriteProduct([FromRoute] long id, [FromBody] SendFavoriteProductModel model)
        {
            if (!await Context.Customers.AnyAsync(c => c.Id == id))
                return NotFound(null);

            if (await Context.CustomerFavoriteProducts.AsQueryable().AnyAsync(fp => fp.ProductId == model.ProductId && fp.CustomerId == id))
                ModelState.AddModelError("ProductId", $"Product with Id {model.ProductId} is already a favorite product of customer with Id {id}.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var relationship = new CustomerFavoriteProduct
            {
                CustomerId = id,
                ProductId = model.ProductId
            };

            await Context.AddAsync(relationship);
            await Context.SaveChangesAsync();

            return Mapper.Map<GetFavoriteProductModel>(relationship);
        }

        [HttpDelete("{id}/favorite_products")]
        public async Task<ActionResult<GetProductModel>> RemoveFavoriteProduct([FromRoute] long id, [FromBody] SendFavoriteProductModel model)
        {
            if (!await Context.Customers.AnyAsync(c => c.Id == id))
                return NotFound(null);

            var relationship = await Context.CustomerFavoriteProducts.AsQueryable().SingleOrDefaultAsync(fp => fp.ProductId == model.ProductId && fp.CustomerId == id);

            if (relationship == null)
                return NotFound(null);

            Context.Remove(relationship);
            await Context.SaveChangesAsync();

            return Ok(null);
        }
    }
}
