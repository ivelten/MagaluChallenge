using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public class ApiTests : TestBase
    {
        public ApiTests(TestServerFixture testServerFixture) 
            : base(testServerFixture)
        {
        }

        [Fact]
        public async Task Health_Endpoint_Must_Return_Healthy_Status()
        {
            using (var client = Server.CreateClient())
            {
                using (var response = await client.GetAsync("/api/health"))
                {
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();

                    content.Should().Be("Healthy");
                }
            }
        }
    }
}
