using AutoMapper;
using Magalu.Challenge.Data;
using Magalu.Challenge.Web.Api.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : DataController<Product, GetProductModel, SendProductModel>
    {
        public ProductController(IConfiguration configuration, MagaluContext context, IMapper mapper)
            : base(configuration, context, mapper, AllowedActions.Get | AllowedActions.GetPage | AllowedActions.Post | AllowedActions.Put)
        {
        }
    }
}
