using Nest;
using Microsoft.Extensions.DependencyInjection;
using ElasticSoDatabase.Services;
using ElasticSoDatabase.Indexes;

namespace ElasticSoDatabase.Utils
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, string url, string defaultIndexName)
        {
            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndexName)
                .DisableDirectStreaming()
                .DefaultMappingFor<PostIndex>(m => m
                    .IndexName(defaultIndexName)
                    .IdProperty(post => post.Id));

            var client = new ElasticClient(settings);

            client.Indices.Create(defaultIndexName, c => c
                .Map<PostIndex>(m => m
                    .AutoMap()));

            services.AddSingleton<IElasticClient>(client);

            services.AddTransient<IPostSearchService, PostSearchService>();
        }
    }
}
