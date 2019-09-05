﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public static class TestServerBuilder
    {
        public static IConfiguration BuildConfiguration()
        {
            return 
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        }

        public static TestServer BuildServer(IConfiguration configuration)
        {
            var builder =
                new WebHostBuilder()
                .UseEnvironment(EnvironmentType.Development.ToString())
                .UseConfiguration(configuration)
                .UseStartup<Startup>();

            return new TestServer(builder);
        }
    }
}
