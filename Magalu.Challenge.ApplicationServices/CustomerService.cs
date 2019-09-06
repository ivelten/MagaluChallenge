using System.Threading.Tasks;
using AutoMapper;
using Magalu.Challenge.Application;
using Magalu.Challenge.Application.Models.Customer;
using Magalu.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Magalu.Challenge.ApplicationServices
{
    public class CustomerService : DataService<Customer, GetCustomerModel, SendCustomerModel>
    {
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationOptions> paginationOptions) 
            : base(unitOfWork, mapper, paginationOptions)
        {
        }

        public override async Task<Result<GetCustomerModel>> SaveAsync(SendCustomerModel model)
        {
            if (await Repository.GetFirstOrDefaultAsync(predicate: c => c.Email == model.Email) != null)
                return Result<GetCustomerModel>.BadRequest($"E-mail address '{model.Email}' is already being used by another customer.");

            return await base.SaveAsync(model);
        }
    }
}
