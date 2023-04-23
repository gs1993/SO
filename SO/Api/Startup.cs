using Api.GraphQL;
using Api.Utils;
using Dawn;
using FluentValidation;
using GrpcPostServer;
using Logic.Contracts;
using Logic.Utils;
using Logic.Utils.Db;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ML;
using PostContentEvaluator.IsPostSpam;
using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AddFluentValidation(services);

            services.AddSwaggerGen();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddMediatR(typeof(DatabaseContext).Assembly);

            AddDbContext(services);

            services.AddCors(options =>
            {
                options.AddPolicy(name: "cors",
                    policy =>
                    {
                        policy.WithOrigins(Configuration.GetValue<string>("Cors:AllowedSite")).AllowAnyHeader().AllowAnyMethod();
                    });
            });

            AddControllers(services);
            AddGraphQL(services);

            services.AddGrpc();

            AddPostContentEvaluator(services, Configuration.GetValue<string>("PredictionModelPath"));
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "SO API"));
            }

            app.UseWebSockets();

            app.UseRouting();
            app.UseCors();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("cors");
                endpoints.MapGraphQL();
                endpoints.MapGrpcService<PostGrpcService>();
            });

            app.UseGraphQLVoyager("/graphql-voyager");
        }


        private void AddDbContext(IServiceCollection services)
        {
            string commandConnectionString = Configuration.GetConnectionString("SO_Database");
            string queryConectionString = Configuration.GetConnectionString("SO_ReadonlyDatabase");
            
            services.AddDbContexts(commandConnectionString, queryConectionString);
        }

        private static void AddGraphQL(IServiceCollection services)
        {
            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddInMemorySubscriptions();
        }

        private static void AddControllers(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                }).ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errorMessages = context.ModelState.Values
                        .Where(x => x.Errors.Any())
                        .Select(x => string.Join(" ", x.Errors.Select(e => e.ErrorMessage)));

                        var error = new EnvelopeError
                        {
                            ErrorMessage = string.Join(Environment.NewLine, errorMessages),
                            TimeGenerated = DateTime.Now
                        };
                        return new BadRequestObjectResult(error);
                    };
                });
        }

        private static void AddFluentValidation(IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<Startup>();
            services.AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
        }

        private static void AddPostContentEvaluator(IServiceCollection services, string modelFilePath)
        {
            Guard.Argument(modelFilePath).NotEmpty().NotWhiteSpace();

            var mlContext = new MLContext();
            var model = mlContext.Model.Load(modelFilePath, out var _);

            services.AddSingleton<IAntySpamPredictionService>(new AntySpamPredictionService(mlContext, model));
        }
    }
}
