using AutoMapper;
using Magalu.Challenge.Application;
using Magalu.Challenge.Data;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magalu.Challenge.ApplicationServices
{
    public interface IReadOnlyDataService<TEntity, TGetModel>
    {
        Task<Result<TGetModel>> GetAsync(params object[] keyValues);

        Task<Result<IEnumerable<TGetModel>>> GetPageAsync(int? page);
    }

    public class ReadOnlyDataService<TEntity, TGetModel> : IReadOnlyDataService<TEntity, TGetModel>
        where TEntity : class
        where TGetModel : class
    {
        protected readonly IReadOnlyRepository<TEntity> Repository;

        protected readonly IMapper Mapper;

        protected readonly int DefaultPageSize;

        public ReadOnlyDataService(IReadOnlyRepository<TEntity> repository, IMapper mapper, IOptions<PaginationOptions> paginationOptions)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            DefaultPageSize = paginationOptions?.Value?.DefaultPageSize ?? throw new ArgumentNullException(nameof(paginationOptions));
        }

        public virtual async Task<Result<TGetModel>> GetAsync(params object[] keyValues)
        {
            var entity = await Repository.FindAsync(keyValues);

            if (entity == null)
                return Result<TGetModel>.NotFound();
            else
                return Result<TGetModel>.Success(Mapper.Map<TGetModel>(entity));
        }

        public virtual async Task<Result<IEnumerable<TGetModel>>> GetPageAsync(int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);

            var entities = await
                Repository.GetPagedListAsync(
                    pageIndex: pageNumber - 1,
                    pageSize: DefaultPageSize);

            return Result<IEnumerable<TGetModel>>.Success(Mapper.Map<IEnumerable<TGetModel>>(entities.Items));
        }
    }
}
