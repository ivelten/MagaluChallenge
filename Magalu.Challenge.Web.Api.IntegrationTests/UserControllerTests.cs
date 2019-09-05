using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Magalu.Challenge.Data.Development;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public class UserControllerTests : TestBase
    {
        public UserControllerTests(TestServerFixture testServerFixture)
            : base(testServerFixture)
        {
        }

        [Theory]
        [InlineData(DatabaseSeeds.UserUsername, DatabaseSeeds.UserPassword)]
        [InlineData(DatabaseSeeds.AdminUsername, DatabaseSeeds.AdminPassword)]
        [InlineData(DatabaseSeeds.CustomerUsername, DatabaseSeeds.CustomerPassword)]
        public async Task Should_Be_Able_To_Authenticate_User(string username, string password)
        {
            using (var client = Server.CreateClient())
            {
                var authenticator = new ApiAuthenticator(client);

                var result = await authenticator.AuthenticateAsync(username, password);

                result.Should().NotBeNull();
                result.Username.Should().Be(username);
                result.JwtToken.Should().NotBeNullOrWhiteSpace();
            }
        }
    }
}
