using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        public TestServerFixture()
        {
            var configuration = TestServerBuilder.BuildConfiguration();

            ConnectionStrings = configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
            PaginationOptions = configuration.GetSection("PaginationOptions").Get<PaginationOptions>();
            SecurityOptions = configuration.GetSection("SecurityOptions").Get<SecurityOptions>();

            Server = TestServerBuilder.BuildServer(configuration);
        }

        public TestServer Server { get; private set; }

        public ConnectionStrings ConnectionStrings { get; private set; }

        public SecurityOptions SecurityOptions { get; private set; }

        public PaginationOptions PaginationOptions { get; private set; }

        public void Dispose()
        {
            Server.Dispose();
        }
    }

    [CollectionDefinition(Name)]
    public class TestServerCollection : ICollectionFixture<TestServerFixture>
    {
        public const string Name = "Test Server Collection";
    }
}
