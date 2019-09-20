using System.Threading.Tasks;
using AutoMapper;
using Magalu.Challenge.Application.Models.Customer;
using Magalu.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Magalu.Challenge.ApplicationServices
{
    public class CustomerService : DataService<Customer, GetCustomerModel, SendCustomerModel>
    {
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork, mapper)
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
