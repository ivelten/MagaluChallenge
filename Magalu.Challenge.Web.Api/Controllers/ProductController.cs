using AutoMapper;
using Magalu.Challenge.Data;
using Magalu.Challenge.Web.Api.Models.Product;
using Magalu.Challenge.Web.Api.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : DataController<Product, GetProductModel, SendProductModel>
    {
        public ProductController(
            IOptions<PaginationOptions> paginationOptions,
            MagaluContext context, 
            IMapper mapper)
            : base(paginationOptions, context, mapper, AllowedActions.Get | AllowedActions.GetPage | AllowedActions.Post | AllowedActions.Put)
        {
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
    }
}
