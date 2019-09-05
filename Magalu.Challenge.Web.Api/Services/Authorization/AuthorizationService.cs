﻿using Magalu.Challenge.Data;
using Magalu.Challenge.Web.Api.Services.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace Magalu.Challenge.Web.Api.Services.Authorization
{
    public interface ICustomerAuthorizationService
    {
        bool CustomerIdIsAuthorized(long id);
    }

    public class CustomerAuthorizationService : ICustomerAuthorizationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CustomerAuthorizationService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private long? TryGetCustomerIdFromClaims()
        {
            var customerIdClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == MagaluClaimTypes.CustomerId);

            if (long.TryParse(customerIdClaim?.Value, out long customerId))
                return customerId;

            return null;
        }

        public bool CustomerIdIsAuthorized(long id)
        {
            var roleClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (roleClaim?.Value == Roles.Administrator)
                return true;

            var customerId = TryGetCustomerIdFromClaims();

            return customerId.HasValue && customerId.Value == id;
        }
    }
}