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

        public DataController(IDataService<TEntity, TGetModel, TSendModel> dataService)
        {
            DataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TGetModel>> Get(Guid id)
        {
            return GetResult(await DataService.GetAsync(id));
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TGetModel>>> GetPage(int? page)
        {
            return GetResult(await DataService.GetPageAsync(page));
        }

        [HttpPost]
        public virtual async Task<ActionResult<TGetModel>> Post([FromBody] TSendModel model)
        {
            var result = await DataService.SaveAsync(model);

            return GetResult(result);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TGetModel>> Put(Guid id, [FromBody] TSendModel model)
        {
            return GetResult(await DataService.UpdateAsync(model, id));
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(Guid id)
        {
            return GetResult(await DataService.DeleteAsync(id));
        }
    }
}
