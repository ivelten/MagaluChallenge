using System.Net.Http;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public abstract class TestBase
    {
        protected readonly HttpClient Client;

        protected readonly ApiAuthenticator Authenticator;

        public TestBase()
        {
            Client = TestServerBuilder.Build().CreateClient();
            Authenticator = new ApiAuthenticator(Client);
        }
    }
}
