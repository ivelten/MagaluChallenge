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
    public class CustomerController : DataController<Customer, GetCustomerModel, PostCustomerModel>
    {
        public CustomerController(IConfiguration configuration, MagaluContext context, IMapper mapper)
            : base(configuration, context, mapper, AllowedActions.All)
        {
        }

        public override async Task<ActionResult<GetCustomerModel>> Post(PostCustomerModel model)
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
    }
}
