using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Logic.Utils.Db.Options
{
    public class DatabaseOptions
    {
        public string ConnectionString { get; init; }
        public int CommandTimeoutInSeconds { get; init; }
        public bool EnableDatailedErrors { get; init; }
        public bool EnableSensitiveDataLogging { get; init; }
    }

    public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
    {
        private const string ConfigurationSectionName = "DatabaseOptions";

        private readonly IConfiguration _configuration;

        public DatabaseOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(DatabaseOptions options)
        {
            _configuration.GetSection(ConfigurationSectionName).Bind(options);
        }
    }
}
