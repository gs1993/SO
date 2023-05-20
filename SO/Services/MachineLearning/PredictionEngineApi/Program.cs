using FastEndpoints.Swagger;
using FastEndpoints;
using Logic.Contracts;
using Microsoft.ML;
using PredictionEngineApi.Services;
using PredictionEngineApi.Utils;

namespace PredictionEngineApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddFastEndpoints();
            builder.Services.SwaggerDocument();

            AddPostContentEvaluator(builder.Services, builder.Configuration.GetValue<string>("PredictionModelPath"));

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseFastEndpoints();

            app.UseOpenApi();
            app.UseSwaggerUi3(s => s.ConfigureDefaults());

            app.Run();
        }

        private static void AddPostContentEvaluator(IServiceCollection services, string modelFilePath)
        {
            if (string.IsNullOrWhiteSpace(modelFilePath))
                throw new ArgumentNullException(modelFilePath, nameof(modelFilePath));

            var mlContext = new MLContext();
            var model = mlContext.Model.Load(modelFilePath, out var _);

            services.AddSingleton<IAntySpamPredictionService>(new AntySpamPredictionService(mlContext, model));
        }
    }
}