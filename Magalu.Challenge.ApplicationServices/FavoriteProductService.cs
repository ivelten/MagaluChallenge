using AutoMapper;
using Magalu.Challenge.Application;
using Magalu.Challenge.Application.Models.Customer;
using Magalu.Challenge.Application.Models.Product;
using Magalu.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magalu.Challenge.ApplicationServices
{
    public interface IFavoriteProductService
    {
        Task<Result<IEnumerable<GetProductModel>>> GetFavoriteProductsAsync(Guid id, int? page);

        Task<Result<GetFavoriteProductModel>> SaveFavoriteProductAsync(Guid id, SendFavoriteProductModel model);

        Task<Result> DeleteFavoriteProduct(Guid customerId, DeleteFavoriteProductModel model);
    }

    public class FavoriteProductService : IFavoriteProductService
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected readonly IRepository<FavoriteProduct> FavoriteProductRepository;

        protected readonly IRepository<Product> ProductRepository;

        protected readonly IMapper Mapper;

        protected readonly int DefaultPageSize;

        protected readonly ICustomerAuthorizationService CustomerAuthorizationService;

        public FavoriteProductService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<PaginationOptions> paginationOptions,
            ICustomerAuthorizationService customerAuthorizationService) 
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            FavoriteProductRepository = unitOfWork.GetRepository<FavoriteProduct>();
            ProductRepository = UnitOfWork.GetRepository<Product>();
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            DefaultPageSize = paginationOptions?.Value?.DefaultPageSize ?? throw new ArgumentNullException(nameof(paginationOptions));
            CustomerAuthorizationService = customerAuthorizationService ?? throw new ArgumentNullException(nameof(customerAuthorizationService));
        }

        public async Task<Result<IEnumerable<GetProductModel>>> GetFavoriteProductsAsync(Guid customerId, int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            var products = await
                FavoriteProductRepository.GetPagedListAsync(
                    selector: p => p.Product, 
                    predicate: p => p.CustomerId == customerId,
                    include: s => s.Include(fp => fp.Product),
                    pageIndex: pageNumber - 1,
                    pageSize: DefaultPageSize);

            return Result<IEnumerable<GetProductModel>>.Success(Mapper.Map<IEnumerable<GetProductModel>>(products.Items));
        }

        public async Task<Result<GetFavoriteProductModel>> SaveFavoriteProductAsync(Guid customerId, SendFavoriteProductModel model)
        {
            if (await ProductRepository.FindAsync(model.ProductId) == null)
                return Result<GetFavoriteProductModel>.BadRequest($"Product with Id {model.ProductId} does not exist.");

            if (await FavoriteProductRepository.GetFirstOrDefaultAsync(predicate: fp => fp.ProductId == model.ProductId && fp.CustomerId == customerId) != null)
                return Result<GetFavoriteProductModel>.BadRequest($"Product with Id {model.ProductId} is already a favorite product of customer with Id {customerId}.");

            var favorite = Mapper.Map<FavoriteProduct>(model);

            favorite.CustomerId = customerId;

            await FavoriteProductRepository.InsertAsync(favorite);
            await UnitOfWork.SaveChangesAsync();

            return Result<GetFavoriteProductModel>.Success(Mapper.Map<GetFavoriteProductModel>(favorite));
        }

        public async Task<Result> DeleteFavoriteProduct(Guid customerId, DeleteFavoriteProductModel model)
        {
            if (!CustomerAuthorizationService.CustomerIdIsAuthorized(customerId))
                return Result.Forbidden();

            if (await ProductRepository.GetFirstOrDefaultAsync(predicate: c => c.Id == customerId) == null)
                return Result.NotFound();

            var relationship = await 
                FavoriteProductRepository.GetFirstOrDefaultAsync(
                    predicate: fp => fp.ProductId == model.ProductId && fp.CustomerId == customerId);

            if (relationship == null)
                return Result.NotFound();

            FavoriteProductRepository.Delete(relationship);

            await UnitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
