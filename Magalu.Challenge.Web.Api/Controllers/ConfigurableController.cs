using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Magalu.Challenge.Web.Api.Controllers
{
    public class ConfigurableController : ControllerBase
    {
        protected readonly IConfiguration Configuration;

        public ConfigurableController(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}
