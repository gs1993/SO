using Api;
using Logic.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Utils
{
    [Collection("Sequential")]
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

        protected async Task<T> GetAndDeserializeResponse<T>(string uri)
        {
            var response = await HttpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseJson);
        }
    }
}
