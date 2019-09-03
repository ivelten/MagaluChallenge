using System.Threading.Tasks;
using AutoMapper;
using Magalu.Challenge.Data;
using Magalu.Challenge.Web.Api.Models.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
                ModelState.AddModelError("Email", $"E-mail address '{model.Email}' is already used by another customer.");

            return await base.Post(model);
        }
    }
}
