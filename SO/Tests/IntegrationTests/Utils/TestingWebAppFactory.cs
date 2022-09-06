using Api;
using Logic.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace IntegrationTests.Utils
{
    public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryEmployeeTest");
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                using var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                TestDataInitializer.Seed(dbContext);
            });
        }
    }
}
