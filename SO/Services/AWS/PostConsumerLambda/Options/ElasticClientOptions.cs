using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace PostConsumerLambda.Options
{
    public class ElasticClientOptions
    {
        public string Url { get; set; }
        public string IndexName { get; set; }
    }

    public class DatabaseOptionsSetup : IConfigureOptions<ElasticClientOptions>
    {
        private const string ConfigurationSectionName = "ElasticClientOptions";

        private readonly IConfiguration _configuration;

        public DatabaseOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(ElasticClientOptions options)
        {
            _configuration.GetSection(ConfigurationSectionName).Bind(options);
        }
    }
}
