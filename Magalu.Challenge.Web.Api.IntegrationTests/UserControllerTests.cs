using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public class UserControllerTests : TestBase
    {
        [Fact]
        public async Task Should_Be_Able_To_Authenticate_User()
        {
            var result = await Authenticator.AuthenticateAsync("user", "user");

            result.Should().NotBeNull();
            result.Username.Should().Be("user");
            result.JwtToken.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Should_Be_Able_To_Authenticate_Administrator()
        {
            var result = await Authenticator.AuthenticateAsync("admin", "admin");

            result.Should().NotBeNull();
            result.Username.Should().Be("admin");
            result.JwtToken.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Should_Be_Able_To_Authenticate_Customer()
        {
            var result = await Authenticator.AuthenticateAsync("john", "john");

            result.Should().NotBeNull();
            result.Username.Should().Be("john");
            result.JwtToken.Should().NotBeNullOrWhiteSpace();
        }
    }
}
