using AutoMapper;
using Magalu.Challenge.Data;
using Magalu.Challenge.Web.Api.Models.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : DataController<Customer, CustomerGetModel, CustomerPostModel>
    {
        public const AllowedHttpVerbs AllowedVerbs = AllowedHttpVerbs.All;

        public CustomerController(IConfiguration configuration, MagaluContext context, IMapper mapper)
            : base(configuration, context, mapper, AllowedVerbs)
        {
        }
    }
}
