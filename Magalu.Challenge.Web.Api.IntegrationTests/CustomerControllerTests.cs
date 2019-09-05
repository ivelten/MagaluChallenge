using FluentAssertions;
using Magalu.Challenge.Data.Development;
using Magalu.Challenge.Web.Api.IntegrationTests.Fixtures;
using Magalu.Challenge.Web.Api.Models.Customer;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public class CustomerControllerTests : TestBase
    {
        public CustomerControllerTests(TestServerFixture testServerFixture) 
            : base(testServerFixture)
        {
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public async Task Get_Should_Return_Expected_Value(long id)
        {
            var response = await Client.GetAsync($"/api/customer/{id}");

            response.EnsureSuccessStatusCode();

            var expected = DatabaseSeeds.Customers[id - 1];
            var actual = await response.Content.ReadAsAsync<GetCustomerModel>();

            actual.Id.Should().Be(id);
            actual.Name.Should().Be(expected.Name);
        }
    }
}
