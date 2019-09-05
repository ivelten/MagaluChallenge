using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    [Collection(TestServerCollection.Name)]
    public abstract class TestBase
    {
        protected readonly TestServer Server;

        protected readonly PaginationOptions PaginationOptions;

        protected readonly SecurityOptions SecurityOptions;

        protected readonly ConnectionStrings ConnectionStrings;

        public TestBase(TestServerFixture testServerFixture)
        {
            Server = testServerFixture.Server;
            PaginationOptions = testServerFixture.PaginationOptions;
            SecurityOptions = testServerFixture.SecurityOptions;
            ConnectionStrings = testServerFixture.ConnectionStrings;
        }
    }
}
