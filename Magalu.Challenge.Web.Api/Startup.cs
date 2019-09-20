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
using Microsoft.OpenApi.Models;

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

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureMvc(services)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);

            services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo { Title = "Magalu Challenge API", Version = "v1" }));

            var connectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("MagaluDatabase");

            services.ConfigureMagaluRepositories(options => options.UseMySql(connectionString));

            // Remove or comment the following line if you don't want to access the external product API as a repository.
            services.ConfigureExternalProductApiRepository();

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

            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            app.UseHealthChecks("/api/health");
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
