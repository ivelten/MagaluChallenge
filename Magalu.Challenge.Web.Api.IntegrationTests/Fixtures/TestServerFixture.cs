using Microsoft.AspNetCore.TestHost;
using System;
using Xunit;

namespace Magalu.Challenge.Web.Api.IntegrationTests.Fixtures
{
    public class TestServerFixture : IDisposable
    {
        public TestServerFixture()
        {
            Server = TestServerBuilder.Build();
        }

        public TestServer Server { get; private set; }

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
