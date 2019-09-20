using AutoMapper;
using Magalu.Challenge.Application;
using Magalu.Challenge.Application.Models.Customer;
using Magalu.Challenge.Application.Models.Product;
using Magalu.Challenge.ApplicationServices.AutoMapper.Profiles;
using Magalu.Challenge.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Magalu.Challenge.ApplicationServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureMagaluApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<SecurityOptions>(configuration.GetSection("SecurityOptions"))
                .Configure<PaginationOptions>(configuration.GetSection("PaginationOptions"))
                .AddHttpContextAccessor()
                .AddScoped<IDataService<Customer, GetCustomerModel, SendCustomerModel>, CustomerService>()
                .AddScoped<IDataService<Product, GetProductModel, SendProductModel>, ProductService>()
                .AddScoped<IDataService<ProductReview, GetProductReviewModel, SendProductReviewModel>, DataService<ProductReview, GetProductReviewModel, SendProductReviewModel>>()
                .AddScoped<IDataService<FavoriteProduct, GetFavoriteProductModel, SendFavoriteProductModel>, DataService<FavoriteProduct, GetFavoriteProductModel, SendFavoriteProductModel>>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<ICustomerAuthorizationService, CustomerAuthorizationService>()
                .AddScoped<IHashingService, BCryptHashingService>()
                .AddScoped<IFavoriteProductService, FavoriteProductService>()
                .AddScoped<IProductReviewService, ProductReviewService>()
                .AddScoped<IReadOnlyDataService<Product, GetProductModel>, ReadOnlyDataService<Product, GetProductModel>>()
                .AddAutoMapper(typeof(DomainToApplicationProfile), typeof(ApplicationToDomainProfile));
        }

        public static IServiceCollection ConfigureMagaluAuthentication(this IServiceCollection services, EnvironmentType environmentType, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
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

            return services;
        }
    }
}
