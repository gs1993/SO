using Dapper;
using Microsoft.Extensions.Configuration;
using SqlToElaticMigratorService.Models.v1;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SqlToElaticMigratorService;

internal class Program
{
    static void Main()
    {
        var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
        var configuration = builder.Build();

        int batchSize = int.Parse(configuration["Elasticsearch:BatchSize"]);
        long migratedPostsCount = 0;
        long errorPostsCount = 0;

        using var connection = new SqlConnection(configuration.GetConnectionString("SO_Database"));

        var client = PostIndex.CreateElasticClient(configuration["Elasticsearch:Url"], configuration["Elasticsearch:IndexName"]);

        var stopwatch = Stopwatch.StartNew();

        int? lastPostId = null;
        while (true)
        {
            var sql = lastPostId.HasValue
            ? $"SELECT TOP {batchSize} * FROM Posts WHERE Id > @LastPostId ORDER BY Id"
            : $"SELECT TOP {batchSize} * FROM Posts ORDER BY Id";

            var posts = connection.Query<Post>(sql, new { LastPostId = lastPostId }).ToList();
            if (!posts.Any())
                break;

            lastPostId = posts.Last().Id;

            var bulkResponse = client.Bulk(b => b.IndexMany(posts));
            if (!bulkResponse.Errors)
                Console.WriteLine(bulkResponse.ServerError);

            int addedCount = bulkResponse.Items.Count(x => x.Status == 200 || x.Status == 201);
            migratedPostsCount += addedCount;
            errorPostsCount += bulkResponse.Items.Count - addedCount;

            Console.WriteLine($"Post migrated: {migratedPostsCount} | errors: {errorPostsCount}");
        }

        stopwatch.Stop();
        Console.WriteLine($"Elapsed time: {stopwatch.Elapsed.Hours}:{stopwatch.Elapsed.Minutes}:{stopwatch.Elapsed.Seconds}");

        Console.ReadLine();
    }
}