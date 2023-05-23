using ChatGptApi.Services;
using ChatGptApi.Utils;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddFastEndpoints();
        builder.Services.SwaggerDocument();
        
        builder.Services.Configure<ChatGptApiSettings>(builder.Configuration.GetSection("ChatGptApiSettings"));

        ChatGptApiSettings apiSettings = new();
        builder.Configuration.GetSection("ChatGptApiSettings").Bind(apiSettings);
        builder.Services.AddSingleton(apiSettings);

        builder.Services.AddHttpClient<IChatGptClient, ChatGptClient>(x =>
        {
            x.BaseAddress = new Uri(apiSettings.Url);
            x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSettings.ApiKey);
        });

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseFastEndpoints();

        app.UseOpenApi();
        app.UseSwaggerUi3(s => s.ConfigureDefaults());

        app.Run();
    }
}