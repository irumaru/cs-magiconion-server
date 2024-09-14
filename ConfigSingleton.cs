using Microsoft.Extensions.Configuration;

class ConfigSingleton
{
  private static ConfigSingleton instance;
  public IConfiguration configuration;

  private ConfigSingleton()
  {
    var builder = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json");

    configuration = builder.Build();
  }

  public static ConfigSingleton GetInstance()
  {
    if (instance == null)
    {
      instance = new ConfigSingleton();
    }
    return instance;
  }
}
