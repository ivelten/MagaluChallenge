using Microsoft.AspNetCore.Hosting;

namespace Magalu.Challenge.Application
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseEnvironment(this IWebHostBuilder builder, EnvironmentType environmentType)
        {
            return builder.UseEnvironment(environmentType.ToString());
        }
    }
}
