using Api;
using Logic.Utils.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace IntegrationTests.Utils
{
    public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            string dockerSqlPort = DockerSqlDatabaseUtilities.EnsureDockerStartedAndGetPortAsync().Result;
            var dockerConnectionString = DockerSqlDatabaseUtilities.GetSqlConnectionString(dockerSqlPort);

            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(
                   new Dictionary<string, string> 
                   { 
                       ["ConnectionStrings:SO_Database"] = dockerConnectionString,
                       ["ConnectionStrings:SO_ReadonlyDatabase"] = dockerConnectionString,
                   });
            });

            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();

                using var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
            });
        }
    }
}
