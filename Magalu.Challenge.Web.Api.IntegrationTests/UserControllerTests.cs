using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Magalu.Challenge.Web.Api.IntegrationTests.Fixtures;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public class UserControllerTests : TestBase
    {
        public UserControllerTests(TestServerFixture testServerFixture)
            : base(testServerFixture)
        {
        }

        [Theory]
        [InlineData("user", "userpw")]
        [InlineData("admin", "adminpw")]
        [InlineData("john", "johnpw")]
        public async Task Should_Be_Able_To_Authenticate_User(string username, string password)
        {
            var result = await Authenticator.AuthenticateAsync(username, password);

            result.Should().NotBeNull();
            result.Username.Should().Be(username);
            result.JwtToken.Should().NotBeNullOrWhiteSpace();
        }
    }
}
