using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public class TestServerStartup : Startup
    {
        public TestServerStartup(IConfiguration configuration, IHostingEnvironment hostingEnvironment) 
            : base(configuration, hostingEnvironment)
        {
        }

        public override IMvcBuilder ConfigureMvc(IServiceCollection services)
        {
            return services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<FakeRemoteIpAddressMiddleware>();
            base.Configure(app);
        }
    }
}
