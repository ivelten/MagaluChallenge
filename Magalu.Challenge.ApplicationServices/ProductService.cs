using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Magalu.Challenge.Application.Models.Product;
using Magalu.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Magalu.Challenge.ApplicationServices
{
    public class ProductService : DataService<Product, GetProductModel, SendProductModel>
    {
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork, mapper)
        {
        }

        public override async Task<Result<IEnumerable<GetProductModel>>> GetPageAsync(int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            var products = await 
                Repository.GetPagedListAsync(
                    include: s => s.Include(p => p.Reviews),
                    selector: p => p, 
                    pageIndex: pageNumber - 1, 
                    pageSize: Constants.PageSize);

            return Result<IEnumerable<GetProductModel>>.Success(Mapper.Map<IEnumerable<GetProductModel>>(products.Items));
        }
    }
}
