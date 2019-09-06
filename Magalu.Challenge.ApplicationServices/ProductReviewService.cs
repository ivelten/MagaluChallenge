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
    public interface IProductReviewService
    {
        Task<Result<IEnumerable<GetProductReviewModel>>> GetProductReviewsOfCustomerAsync(long customerId, int? page);

        Task<Result<IEnumerable<GetProductReviewModel>>> GetProductReviewsOfProductAsync(long productId, int? page);

        Task<Result<GetProductReviewModel>> SaveReviewAsync(long productId, SendProductReviewModel model);

        Task<Result<GetProductReviewModel>> UpdateReviewAsync(long productId, SendProductReviewModel model);

        Task<Result> DeleteReviewAsync(long id, DeleteProductReviewModel model);
    }

    public class ProductReviewService : IProductReviewService
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected readonly IRepository<ProductReview> ProductReviewRepository;

        protected readonly IRepository<Product> ProductRepository;

        protected readonly IRepository<Customer> CustomerRepository;

        protected readonly IMapper Mapper;

        protected readonly int DefaultPageSize;

        protected readonly ICustomerAuthorizationService CustomerAuthorizationService;

        public ProductReviewService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<PaginationOptions> paginationOptions,
            ICustomerAuthorizationService customerAuthorizationService)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            ProductReviewRepository = unitOfWork.GetRepository<ProductReview>();
            ProductRepository = unitOfWork.GetRepository<Product>();
            CustomerRepository = unitOfWork.GetRepository<Customer>();
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            DefaultPageSize = paginationOptions?.Value?.DefaultPageSize ?? throw new ArgumentNullException(nameof(paginationOptions));
            CustomerAuthorizationService = customerAuthorizationService ?? throw new ArgumentNullException(nameof(customerAuthorizationService));
        }

        public async Task<Result<IEnumerable<GetProductReviewModel>>> GetProductReviewsOfCustomerAsync(long customerId, int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            var reviews = await
                ProductReviewRepository.GetPagedListAsync(
                    selector: r => r,
                    predicate: r => r.CustomerId == customerId,
                    pageIndex: pageNumber - 1,
                    pageSize: DefaultPageSize);

            return Result<IEnumerable<GetProductReviewModel>>.Success(Mapper.Map<IEnumerable<GetProductReviewModel>>(reviews.Items));
        }

        public async Task<Result<IEnumerable<GetProductReviewModel>>> GetProductReviewsOfProductAsync(long productId, int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            var reviews = await
                ProductReviewRepository.GetPagedListAsync(
                    selector: r => r,
                    predicate: r => r.ProductId == productId,
                    pageIndex: pageNumber - 1,
                    pageSize: DefaultPageSize);

            return Result<IEnumerable<GetProductReviewModel>>.Success(Mapper.Map<IEnumerable<GetProductReviewModel>>(reviews.Items));
        }

        public async Task<Result<GetProductReviewModel>> SaveReviewAsync(long productId, SendProductReviewModel model)
        {
            if (!CustomerAuthorizationService.CustomerIdIsAuthorized(productId))
                return Result<GetProductReviewModel>.Forbidden();

            if (await ProductRepository.GetFirstOrDefaultAsync(predicate: p => p.Id == productId) == null)
                return Result<GetProductReviewModel>.BadRequest($"Product with Id {productId} does not exist.");

            if (await CustomerRepository.GetFirstOrDefaultAsync(predicate: c => c.Id == model.CustomerId) == null)
                return Result<GetProductReviewModel>.BadRequest($"Customer with Id {model.CustomerId} does not exist.");

            if (await ProductReviewRepository.GetFirstOrDefaultAsync(predicate: fp => fp.CustomerId == model.CustomerId && fp.ProductId == productId) != null)
                return Result<GetProductReviewModel>.BadRequest($"Customer with id {model.CustomerId} already has a review for product with id {productId}.");

            var review = Mapper.Map<ProductReview>(model);

            review.ProductId = productId;

            await ProductReviewRepository.InsertAsync(review);
            await UnitOfWork.SaveChangesAsync();

            return Result<GetProductReviewModel>.Success(Mapper.Map<GetProductReviewModel>(review));
        }

        public async Task<Result<GetProductReviewModel>> UpdateReviewAsync(long productId, SendProductReviewModel model)
        {
            if (!CustomerAuthorizationService.CustomerIdIsAuthorized(model.CustomerId))
                return Result<GetProductReviewModel>.Forbidden();

            if (await ProductRepository.GetFirstOrDefaultAsync(predicate: p => p.Id == productId) == null)
                return Result<GetProductReviewModel>.BadRequest($"Product with Id {productId} does not exist.");

            if (await CustomerRepository.GetFirstOrDefaultAsync(predicate: c => c.Id == model.CustomerId) == null)
                return Result<GetProductReviewModel>.BadRequest($"Customer with Id {model.CustomerId} does not exist.");

            var review = await ProductReviewRepository.GetFirstOrDefaultAsync(predicate: fp => fp.CustomerId == model.CustomerId && fp.ProductId == productId);

            if (review == null)
                return Result<GetProductReviewModel>.NotFound();

            Mapper.Map(model, review);

            await UnitOfWork.SaveChangesAsync();

            return Result<GetProductReviewModel>.Success(Mapper.Map<GetProductReviewModel>(review));
        }

        public async Task<Result> DeleteReviewAsync(long id, DeleteProductReviewModel model)
        {
            if (CustomerAuthorizationService.CustomerIdIsAuthorized(id))
                return Result.Forbidden();

            if (await ProductRepository.GetFirstOrDefaultAsync(predicate: c => c.Id == id) == null)
                return Result.NotFound();

            var review = await ProductReviewRepository.GetFirstOrDefaultAsync(predicate: fp => fp.CustomerId == model.CustomerId && fp.ProductId == id);

            if (review == null)
                return Result.NotFound();

            ProductReviewRepository.Delete(review);

            await UnitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
