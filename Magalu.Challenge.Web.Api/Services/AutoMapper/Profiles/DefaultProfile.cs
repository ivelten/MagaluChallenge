using AutoMapper;
using Magalu.Challenge.Data;
using Magalu.Challenge.Web.Api.Models.Customer;
using Magalu.Challenge.Web.Api.Models.Product;
using Magalu.Challenge.Web.Api.Models.Shared;
using System.Linq;

namespace Magalu.Challenge.Web.Api.Services.AutoMapper.Profiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<Product, GetProductModel>()
                .ForMember(m => m.ReviewScore, s => s.MapFrom(p => p.Reviews.AverageOrDefault(r => r.Score)));

            CreateMap<SendProductModel, Product>();

            CreateMap<Customer, GetCustomerModel>();

            CreateMap<SendCustomerModel, Customer>();

            CreateMap<SendFavoriteProductModel, FavoriteProduct>();

            CreateMap<FavoriteProduct, GetFavoriteProductModel>();

            CreateMap<SendProductReviewModel, ProductReview>();

            CreateMap<ProductReview, GetProductReviewModel>();
        }
    }
}
