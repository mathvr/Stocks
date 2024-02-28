namespace stocks.Services.Session;

public class ConfigurationHelper : IConfigurationHelper
{
    private IConfigurationRoot _builder = GetConfigBuilder();
    
    private static IConfigurationRoot GetConfigBuilder()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json", false, false)
            .Build();
    }

    public string GetJwtToken()
    {
        return _builder["JWTSettings:TokenKey"];
    }
}