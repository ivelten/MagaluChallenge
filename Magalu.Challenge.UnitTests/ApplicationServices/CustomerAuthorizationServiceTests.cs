using Magalu.Challenge.ApplicationServices;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System;
using System.Security.Claims;
using ClaimTypes = Magalu.Challenge.Application.ClaimTypes;
using Xunit;
using FluentAssertions;

namespace Magalu.Challenge.UnitTests.ApplicationServices
{
    public class CustomerAuthorizationServiceTests
    {
        [Theory]
        [InlineData("6e034808-df29-4a70-9344-3f24129953df", "6e034808-df29-4a70-9344-3f24129953df")]
        [InlineData("6e034808-df29-4a70-9344-3f24129953df", "2e73aa33-2a45-4d29-bf66-955a9a0b7236")]
        public void CustomerIdIsAuthorized_Should_Validate_Authorized_Customer_Based_On_CustomerId_Claim(string authorized, string input)
        {
            var httpContext = Substitute.For<HttpContext>();

            var authorizedId = new Guid(authorized);
            var inputId = new Guid(input);

            var claims = new Claim[] { new Claim(ClaimTypes.CustomerId, authorized) };

            httpContext.User.Returns(new ClaimsPrincipal(new ClaimsIdentity(claims)));

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            httpContextAccessor.HttpContext.Returns(httpContext);

            var sut = new CustomerAuthorizationService(httpContextAccessor);

            sut.CustomerIdIsAuthorized(inputId).Should().Be(authorizedId == inputId);
        }
    }
}
