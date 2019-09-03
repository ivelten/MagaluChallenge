using AutoMapper;
using Magalu.Challenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Magalu.Challenge.Web.Api.Controllers
{
    public abstract class DataController<TEntity, TGetModel, TPostModel> : ConfigurableController where TEntity : class
    {
        protected readonly MagaluContext Context;

        protected readonly IMapper Mapper;

        protected readonly int DefaultPageSize;

        private readonly AllowedActions allowedActions;

        public DataController(IConfiguration configuration, MagaluContext context, IMapper mapper, AllowedActions allowedActions)
            : base(configuration)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            DefaultPageSize = configuration.GetSection("Pagination").GetValue<int>("DefaultPageSize");
            this.allowedActions = allowedActions;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TGetModel>> Get(long id)
        {
            if (!allowedActions.HasFlag(AllowedActions.Get))
                return NotFound(null);

            var entity = await Context.Set<TEntity>().FindAsync(id);

            if (entity == null)
                return NotFound(null);
            else
                return Mapper.Map<TGetModel>(entity);
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TGetModel>>> GetPage(int page)
        {
            if (!allowedActions.HasFlag(AllowedActions.GetPage))
                return NotFound(null);

            var entities = await Context.Set<TEntity>().AsQueryable().SelectPage(page, DefaultPageSize).ToArrayAsync();
            return Mapper.Map<TGetModel[]>(entities);
        }

        [HttpPost]
        public virtual async Task<ActionResult<TGetModel>> Post([FromBody] TPostModel model)
        {
            if (!allowedActions.HasFlag(AllowedActions.Post))
                return NotFound(null);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = Mapper.Map<TEntity>(model);

            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();

            return Mapper.Map<TGetModel>(entity);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TGetModel>> Put(long id, [FromBody] TPostModel model)
        {
            if (!allowedActions.HasFlag(AllowedActions.Put))
                return NotFound(null);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await Context.Set<TEntity>().FindAsync(id);

            if (entity == null)
                return NotFound(null);

            Mapper.Map(model, entity);

            await Context.SaveChangesAsync();

            return Mapper.Map<TGetModel>(entity);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(long id)
        {
            if (!allowedActions.HasFlag(AllowedActions.Put))
                return NotFound(null);

            var entity = await Context.Set<TEntity>().FindAsync(id);

            if (entity == null)
                return NotFound(null);

            Context.Set<TEntity>().Remove(entity);

            await Context.SaveChangesAsync();

            return Ok(null);
        }
    }
}
