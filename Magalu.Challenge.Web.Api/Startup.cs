using AutoMapper;
using Magalu.Challenge.Data;
using Magalu.Challenge.Data.Development;
using Magalu.Challenge.Security;
using Magalu.Challenge.Shared.Abstractions;
using Magalu.Challenge.Web.Api.Services.Authentication;
using Magalu.Challenge.Web.Api.Services.Authorization;
using Magalu.Challenge.Web.Api.Services.AutoMapper.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        private readonly bool isDevelopmentEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof(hostingEnvironment));

            isDevelopmentEnvironment = hostingEnvironment.IsDevelopment();
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
            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));

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

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = !isDevelopmentEnvironment;
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

            if (isDevelopmentEnvironment)
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
            if (isDevelopmentEnvironment)
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
