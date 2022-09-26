using Api;
using Logic.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Xunit;

namespace IntegrationTests.Utils
{
    public class ControllerIntegrationTestsBase : IClassFixture<TestingWebAppFactory<Program>>
    {
        protected readonly HttpClient HttpClient;

        public ControllerIntegrationTestsBase(TestingWebAppFactory<Program> factory)
        {
            HttpClient = factory.CreateClient();
            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            TestDataInitializer.Seed(dbContext);
        }
    }
}
