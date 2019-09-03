using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Magalu.Challenge.Data;
using Magalu.Challenge.Web.Api.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Magalu.Challenge.Core;
using Microsoft.EntityFrameworkCore;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : DataController
    {
        public ProductController(IConfiguration configuration, MagaluContext context, IMapper mapper)
            : base(configuration, context, mapper)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProductModel>>> GetPage(int page)
        {
            var productsPage = await Context.Products.SelectPage(page, DefaultPageSize).AsQueryable().ToArrayAsync();
            return Mapper.Map<GetProductModel[]>(productsPage);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductModel>> Get(int id)
        {
            var product = await Context.Products.SingleOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();
            else
                return Mapper.Map<GetProductModel>(product);
        }

        [HttpPost]
        public async Task<ActionResult<GetProductModel>> Post([FromBody] PostProductModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = Mapper.Map<Product>(model);

            await Context.AddAsync(product);
            await Context.SaveChangesAsync();

            return Mapper.Map<GetProductModel>(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GetProductModel>> Put(int id, [FromBody] PostProductModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await Context.Products.SingleOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            product.Image = model.Image;
            product.Price = model.Price;
            product.Title = model.Title;
            product.Brand = model.Brand;

            await Context.SaveChangesAsync();

            return Mapper.Map<GetProductModel>(product);
        }
    }
}
