using Xunit;

namespace Magalu.Challenge.Web.Api.IntegrationTests.Fixtures
{
    [CollectionDefinition(Name)]
    public class TestServerCollection : ICollectionFixture<TestServerFixture>
    {
        public const string Name = "Test Server Collection";
    }
}
