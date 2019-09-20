using Magalu.Challenge.ApplicationServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magalu.Challenge.Web.Api.Controllers
{
    public class ReadOnlyDataController<TEntity, TGetModel> : ApplicationController where TEntity : class
    {
        protected readonly IReadOnlyDataService<TEntity, TGetModel> DataService;

        public ReadOnlyDataController(IReadOnlyDataService<TEntity, TGetModel> dataService)
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
    }
}
