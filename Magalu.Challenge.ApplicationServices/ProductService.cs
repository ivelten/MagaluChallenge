using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Magalu.Challenge.Application;
using Magalu.Challenge.Application.Models.Product;
using Magalu.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Magalu.Challenge.ApplicationServices
{
    public class ProductService : DataService<Product, GetProductModel, SendProductModel>
    {
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationOptions> paginationOptions) 
            : base(unitOfWork, mapper, paginationOptions)
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
                    pageSize: DefaultPageSize);

            return Result<IEnumerable<GetProductModel>>.Success(Mapper.Map<IEnumerable<GetProductModel>>(products.Items));
        }
    }
}
