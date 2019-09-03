using AutoMapper;
using Magalu.Challenge.Data;
using Magalu.Challenge.Web.Api.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : DataController<Product, GetProductModel, PostProductModel>
    {
        public const AllowedHttpVerbs AllowedVerbs = AllowedHttpVerbs.Get | AllowedHttpVerbs.GetPage | AllowedHttpVerbs.Post | AllowedHttpVerbs.Put;

        public ProductController(IConfiguration configuration, MagaluContext context, IMapper mapper)
            : base(configuration, context, mapper, AllowedVerbs)
        {
        }
    }
}
