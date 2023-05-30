using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using PostConsumerLambda.Options;

namespace PostConsumerLambda
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(_configuration);

            ConfigureApplicationServices(services);

            ConfigureElasticClient(services);

            IServiceProvider provider = services.BuildServiceProvider();

            return provider;
        }

        private void ConfigureElasticClient(ServiceCollection services)
        {
            var uri = _configuration.GetValue<string>("ElasticClientOptions:Uri");
            var indexName = _configuration.GetValue<string>("ElasticClientOptions:IndexName");

            var connectionSettings = new ConnectionSettings(new Uri(uri))
                .DefaultIndex(indexName);

            services.AddSingleton<IElasticClient>(new ElasticClient(connectionSettings));
        }

        private void ConfigureApplicationServices(ServiceCollection services)
        {
            var awsOptions = _configuration.GetAWSOptions();

            services.AddDefaultAWSOptions(awsOptions);
            services.AddSingleton<ILambdaEntryPoint, LambdaEntryPoint>();
        }
    }
}
