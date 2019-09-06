using AutoMapper;
using Magalu.Challenge.Application.Models.Customer;
using Magalu.Challenge.Application.Models.Product;
using Magalu.Challenge.Domain.Entities;
using System.Linq;

namespace Magalu.Challenge.ApplicationServices.AutoMapper.Profiles
{
    public class DomainToApplicationProfile : Profile
    {
        public DomainToApplicationProfile()
        {
            CreateMap<Product, GetProductModel>().ForMember(m => m.ReviewScore, s => s.MapFrom(p => p.Reviews.AverageOrDefault(r => r.Score)));
            CreateMap<Customer, GetCustomerModel>();
            CreateMap<FavoriteProduct, GetFavoriteProductModel>();
            CreateMap<ProductReview, GetProductReviewModel>();
        }
    }
}
