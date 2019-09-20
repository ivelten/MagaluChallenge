using Magalu.Challenge.Application;
using Magalu.Challenge.ApplicationServices;
using Magalu.Challenge.Data;
using Magalu.Challenge.Data.Development;
using Magalu.Challenge.Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace Magalu.Challenge.Web.Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        private readonly EnvironmentType environmentType;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof(hostingEnvironment));

            environmentType = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), hostingEnvironment.EnvironmentName);
        }

        public virtual IMvcBuilder ConfigureMvc(IServiceCollection services)
        {
            return services.AddMvc();
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            ConfigureMvc(services)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);

            var connectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("MagaluDatabase");
            services.ConfigureMagaluRepositories(options => options.UseMySql(connectionString));

            services.ConfigureMagaluApplicationServices(configuration);

            services.AddHealthChecks();

            services.ConfigureMagaluAuthentication(environmentType, configuration);

            if (environmentType == EnvironmentType.Development)
                services.ConfigureDevelopmentDatabase();
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            if (environmentType == EnvironmentType.Development)
                app.UseDeveloperExceptionPage();

            if (environmentType == EnvironmentType.Production)
                app.UseHsts();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseHealthChecks("/api/health");
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
