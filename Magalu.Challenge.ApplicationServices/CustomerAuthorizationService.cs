using Magalu.Challenge.Application;
using Magalu.Challenge.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Magalu.Challenge.ApplicationServices
{
    public interface ICustomerAuthorizationService
    {
        bool CustomerIdIsAuthorized(Guid id);
    }

    public class CustomerAuthorizationService : ICustomerAuthorizationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CustomerAuthorizationService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private Guid? TryGetCustomerIdFromClaims()
        {
            var customerIdClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.CustomerId);

            if (Guid.TryParse(customerIdClaim?.Value, out Guid customerId))
                return customerId;

            return null;
        }

        public bool CustomerIdIsAuthorized(Guid id)
        {
            var roleClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (roleClaim?.Value == Roles.Administrator)
                return true;

            var customerId = TryGetCustomerIdFromClaims();

            return customerId.HasValue && customerId.Value == id;
        }
    }
}
