using AutoMapper;
using Magalu.Challenge.Application;
using Magalu.Challenge.ApplicationServices;
using Magalu.Challenge.ApplicationServices.AutoMapper.Profiles;
using Magalu.Challenge.Data;
using Magalu.Challenge.Data.Development;
using Magalu.Challenge.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);

            services.AddHttpContextAccessor();

            services.AddTransient<IHashingService, BCryptHashingService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ICustomerAuthorizationService, CustomerAuthorizationService>();

            services.Configure<SecurityOptions>(configuration.GetSection("SecurityOptions"));
            services.Configure<PaginationOptions>(configuration.GetSection("PaginationOptions"));

            services.AddMagaluRepositories(configuration.GetSection("ConnectionStrings").GetValue<string>("MagaluDatabase"));

            services.AddAutoMapper(typeof(DomainToApplicationProfile), typeof(ApplicationToDomainProfile));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddHealthChecks();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = environmentType != EnvironmentType.Production;
                    options.SaveToken = true;

                    var key = Convert.FromBase64String(configuration.GetSection("SecurityOptions").GetValue<string>("JwtSecret"));

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            if (environmentType == EnvironmentType.Development)
            {
                var serviceProvider = services.BuildServiceProvider();

                var context = serviceProvider.GetService<MagaluContext>();
                var hashingService = serviceProvider.GetService<IHashingService>();

                var databaseInitializer = new MagaluContextDatabaseInitializer(context, hashingService);

                databaseInitializer.InitializeDevelopmentEnvironmentDatabase();
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (environmentType == EnvironmentType.Development)
                app.UseDeveloperExceptionPage();


            if (environmentType == EnvironmentType.Production)
            {
                app.UseHsts();
            }

            app.UseHealthChecks("/api/health");
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
