using CdcWorkerService;
using CdcWorkerService.Db;
using CdcWorkerService.Db.Models;
using CdcWorkerService.EsClient;
using CdcWorkerService.Strategy;
using Microsoft.EntityFrameworkCore;
using Nest;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var isDevelopment = hostContext.HostingEnvironment.IsDevelopment();

        AddDbContext(hostContext, services, isDevelopment);

        AddElasticSearch(hostContext, services);

        services.AddSingleton(sp =>
        {
            return new Dictionary<OperationEnum, CdcStrategy>
            {
                { OperationEnum.Delete, new DeleteCdcStrategy() },
                { OperationEnum.Insert, new InsertCdcStrategy() },
                { OperationEnum.ValueAfterUpdate, new ValueAfterUpdateCdcStrategy() }
            };
        });

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();

static void AddDbContext(HostBuilderContext hostContext, IServiceCollection services, bool isDevelopment)
{
    services.AddLogging(loggingBuilder =>
    {
        if (isDevelopment)
        {
            loggingBuilder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == Microsoft.Extensions.Logging.LogLevel.Information);
        }
        loggingBuilder.AddConsole();
    });

    services.AddDbContext<DatabaseContext>(options =>
    {
        options.UseSqlServer(hostContext.Configuration.GetConnectionString("SO_Database"));
    });
}

static void AddElasticSearch(HostBuilderContext hostContext, IServiceCollection services)
{
    services.AddSingleton(sp =>
    {
        var url = hostContext.Configuration["Elasticsearch:Url"];

        var settings = new ConnectionSettings(new Uri(url))
            .DefaultMappingFor<PostIndex>(m => m
                .IndexName("posts_index")
                .IdProperty(post => post.Id));

        var client = new ElasticClient(settings);

        client.Indices.Create("posts_index", c => c
            .Map<PostIndex>(m => m
                .AutoMap()
                .Properties(ps => ps
                    .Keyword(t => t.Name(n => n.Tags)))));

        return client;
    });
}