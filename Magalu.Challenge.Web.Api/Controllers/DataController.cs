using AutoMapper;
using Magalu.Challenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Magalu.Challenge.Web.Api.Controllers
{
    public abstract class DataController : ControllerBase
    {
        protected readonly IConfiguration Configuration;

        protected readonly MagaluContext Context;

        protected readonly IMapper Mapper;

        protected readonly int DefaultPageSize;

        public DataController(IConfiguration configuration, MagaluContext context, IMapper mapper)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            DefaultPageSize = Configuration.GetSection("Pagination").GetValue<int>("DefaultPageSize");
        }
    }
}
