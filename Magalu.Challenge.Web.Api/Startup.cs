using AutoMapper;
using Magalu.Challenge.Data;
using Magalu.Challenge.Data.Development;
using Magalu.Challenge.Security;
using Magalu.Challenge.Shared.Abstractions;
using Magalu.Challenge.Web.Api.Services.AutoMapper.Profiles;
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

        private readonly IHostingEnvironment hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);

            services.AddTransient<IHashingService, BCryptHashingService>();
            services.AddTransient<MagaluContextDatabaseInitializer>();

            services.AddDbContext<MagaluContext>(options =>
            {
                var connectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("MagaluDatabase");

                options.UseMySql(connectionString);
                options.UseLazyLoadingProxies();
            });

            services.AddAutoMapper(typeof(DefaultProfile));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            if (hostingEnvironment.IsDevelopment())
            {
                var serviceProvider = services.BuildServiceProvider();
                var databaseInitializer = serviceProvider.GetService<MagaluContextDatabaseInitializer>();

                databaseInitializer.InitializeDevelopmentEnvironmentDatabase();
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (hostingEnvironment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
