using Magalu.Challenge.Application;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    [Collection(TestServerCollection.Name)]
    public abstract class TestBase
    {
        protected readonly TestServer Server;

        protected readonly SecurityOptions SecurityOptions;

        public TestBase(TestServerFixture testServerFixture)
        {
            Server = testServerFixture.Server;
            SecurityOptions = testServerFixture.SecurityOptions;
        }
    }
}
