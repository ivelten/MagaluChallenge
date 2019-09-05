using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public class ApiTests : TestBase
    {
        [Fact]
        public async Task Health_Endpoint_Must_Return_Healthy_Status()
        {
            using (var response = await Client.GetAsync("/api/health"))
            {
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                content.Should().Be("Healthy");
            }
        }
    }
}
