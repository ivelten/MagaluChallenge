using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public static class TestServerBuilder
    {
        public static TestServer Build()
        {
            var configuration = 
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var builder =
                new WebHostBuilder()
                .UseEnvironment("Development")
                .UseConfiguration(configuration)
                .UseStartup<Startup>();

            return new TestServer(builder);
        }
    }
}
