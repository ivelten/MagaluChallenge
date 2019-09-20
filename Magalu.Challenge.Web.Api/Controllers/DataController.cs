using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Magalu.Challenge.ApplicationServices;

namespace Magalu.Challenge.Web.Api.Controllers
{
    public abstract class DataController<TEntity, TGetModel, TSendModel> : ApplicationController where TEntity : class
    {
        protected readonly IDataService<TEntity, TGetModel, TSendModel> DataService;

        private readonly AllowedActions allowedActions;

        public DataController(
            IDataService<TEntity, TGetModel, TSendModel> dataService,
            AllowedActions allowedActions)
        {
            DataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            this.allowedActions = allowedActions;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TGetModel>> Get(Guid id)
        {
            if (!allowedActions.HasFlag(AllowedActions.Get))
                return NotFound(null);

            return GetResult(await DataService.GetAsync(id));
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TGetModel>>> GetPage(int? page)
        {
            if (!allowedActions.HasFlag(AllowedActions.GetPage))
                return NotFound(null);

            return GetResult(await DataService.GetPageAsync(page));
        }

        [HttpPost]
        public virtual async Task<ActionResult<TGetModel>> Post([FromBody] TSendModel model)
        {
            if (!allowedActions.HasFlag(AllowedActions.Post))
                return NotFound(null);

            var result = await DataService.SaveAsync(model);

            return GetResult(result);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TGetModel>> Put(Guid id, [FromBody] TSendModel model)
        {
            if (!allowedActions.HasFlag(AllowedActions.Put))
                return NotFound(null);

            return GetResult(await DataService.UpdateAsync(model, id));
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(Guid id)
        {
            if (!allowedActions.HasFlag(AllowedActions.Put))
                return NotFound(null);

            return GetResult(await DataService.DeleteAsync(id));
        }
    }
}
