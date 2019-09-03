using AutoMapper;
using Magalu.Challenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Magalu.Challenge.Core;
using System.Linq;

namespace Magalu.Challenge.Web.Api.Controllers
{
    public abstract class DataController<TEntity, TGetModel, TPostModel> : ControllerBase where TEntity : class
    {
        protected readonly IConfiguration Configuration;

        protected readonly MagaluContext Context;

        protected readonly IMapper Mapper;

        protected readonly int DefaultPageSize;

        private readonly AllowedHttpVerbs allowedVerbs;

        public DataController(IConfiguration configuration, MagaluContext context, IMapper mapper, AllowedHttpVerbs allowedVerbs)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            DefaultPageSize = Configuration.GetSection("Pagination").GetValue<int>("DefaultPageSize");
            this.allowedVerbs = allowedVerbs;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TGetModel>> Get(int id)
        {
            if (!allowedVerbs.HasFlag(AllowedHttpVerbs.Get))
                return NotFound();

            var entity = await Context.Set<TEntity>().FindAsync(id);

            if (entity == null)
                return NotFound();
            else
                return Mapper.Map<TGetModel>(entity);
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TGetModel>>> GetPage(int page)
        {
            if (!allowedVerbs.HasFlag(AllowedHttpVerbs.GetPage))
                return NotFound();

            var entities = await Context.Set<TEntity>().SelectPage(page, DefaultPageSize).AsQueryable().ToArrayAsync();
            return Mapper.Map<TGetModel[]>(entities);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TGetModel>> Put(int id, [FromBody] TPostModel model)
        {
            if (!allowedVerbs.HasFlag(AllowedHttpVerbs.Put))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await Context.Set<TEntity>().FindAsync(id);

            if (entity == null)
                return NotFound();

            Mapper.Map(model, entity);

            await Context.SaveChangesAsync();

            return Mapper.Map<TGetModel>(entity);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(int id)
        {
            if (!allowedVerbs.HasFlag(AllowedHttpVerbs.Put))
                return NotFound();

            var entity = await Context.Set<TEntity>().FindAsync(id);

            if (entity == null)
                return NotFound();

            Context.Set<TEntity>().Remove(entity);

            await Context.SaveChangesAsync();

            return Ok();
        }
    }
}
