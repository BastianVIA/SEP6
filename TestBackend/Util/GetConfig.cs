using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace TestBackend.Util;

public static class GetConfig
{
    private static IConfiguration _configuration;

    static GetConfig()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true)
            .Build();
    }

    public static IConfiguration GetTestConfig()
    {
        return _configuration;
    }
}