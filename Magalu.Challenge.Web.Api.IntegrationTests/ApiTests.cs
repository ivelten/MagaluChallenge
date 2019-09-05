using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public class ApiTests
    {
        private readonly HttpClient client;

        public ApiTests()
        {
            client = TestServerBuilder.Build().CreateClient();
        }

        [Fact]
        public async Task Health_Endpoint_Must_Return_Healthy_Status()
        {
            using (var response = await client.GetAsync("/api/health"))
            {
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                Assert.Equal("Healthy", content);
            }
        }
    }
}
