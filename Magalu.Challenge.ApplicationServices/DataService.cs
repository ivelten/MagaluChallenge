using AutoMapper;
using Magalu.Challenge.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magalu.Challenge.ApplicationServices
{
    public interface IDataService<TEntity, TGetModel, TSendModel>
    {
        Task<Result<TGetModel>> GetAsync(params object[] keyValues);

        Task<Result<IEnumerable<TGetModel>>> GetPageAsync(int? page);

        Task<Result<TGetModel>> SaveAsync(TSendModel model);

        Task<Result<TGetModel>> UpdateAsync(TSendModel model, params object[] keyValues);

        Task<Result> DeleteAsync(params object[] keyValues);
    }

    public class DataService<TEntity, TGetModel, TSendModel> : IDataService<TEntity, TGetModel, TSendModel> 
        where TEntity : class 
        where TGetModel : class
        where TSendModel : class
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected readonly IRepository<TEntity> Repository;

        protected readonly IMapper Mapper;

        public DataService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            Repository = unitOfWork.GetRepository<TEntity>();
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
                    pageSize: Constants.PageSize);

            return Result<IEnumerable<TGetModel>>.Success(Mapper.Map<IEnumerable<TGetModel>>(entities.Items));
        }

        public virtual async Task<Result<TGetModel>> SaveAsync(TSendModel model)
        {
            var entity = Mapper.Map<TEntity>(model);

            await Repository.InsertAsync(entity);
            await UnitOfWork.SaveChangesAsync();

            return Result<TGetModel>.Success(Mapper.Map<TGetModel>(entity));
        }

        public virtual async Task<Result<TGetModel>> UpdateAsync(TSendModel model, params object[] keyValues)
        {
            var entity = await Repository.FindAsync(keyValues);

            if (entity == null)
                Result.NotFound();

            Mapper.Map(model, entity);

            await UnitOfWork.SaveChangesAsync();

            return Result<TGetModel>.Success(Mapper.Map<TGetModel>(entity));
        }

        public virtual async Task<Result> DeleteAsync(params object[] keyValues)
        {
            var entity = await Repository.FindAsync(keyValues);

            if (entity == null)
                Result.NotFound();

            Repository.Delete(entity);

            await UnitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
