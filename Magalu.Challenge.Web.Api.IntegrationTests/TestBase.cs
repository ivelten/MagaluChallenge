using Magalu.Challenge.Web.Api.IntegrationTests.Fixtures;
using System.Net.Http;
using Xunit;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    [Collection(TestServerCollection.Name)]
    public abstract class TestBase
    {
        protected readonly HttpClient Client;

        protected readonly ApiAuthenticator Authenticator;

        public TestBase(TestServerFixture testServerFixture)
        {
            Client = testServerFixture.Server.CreateClient();
            Authenticator = new ApiAuthenticator(Client);
        }
    }
}
