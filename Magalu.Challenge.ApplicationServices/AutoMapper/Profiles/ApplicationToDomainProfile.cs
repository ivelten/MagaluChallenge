using AutoMapper;
using Magalu.Challenge.Application.Models.Customer;
using Magalu.Challenge.Application.Models.Product;
using Magalu.Challenge.Domain.Entities;

namespace Magalu.Challenge.ApplicationServices.AutoMapper.Profiles
{
    public class ApplicationToDomainProfile : Profile
    {
        public ApplicationToDomainProfile()
        {
            CreateMap<SendProductModel, Product>();
            CreateMap<SendCustomerModel, Customer>();
            CreateMap<SendFavoriteProductModel, FavoriteProduct>();
            CreateMap<SendProductReviewModel, ProductReview>();            
        }
    }
}
