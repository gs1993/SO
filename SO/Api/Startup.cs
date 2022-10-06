using Api.GraphQL;
using Api.Utils;
using FluentValidation;
using Logic.Utils;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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
            services.AddCors();
            services.AddControllers()
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

            services.AddValidatorsFromAssemblyContaining<Startup>();
            services.AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();

            services.AddSwaggerGen();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddMediatR(typeof(DatabaseContext).Assembly);

            AddDbContext(services);

            services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddInMemorySubscriptions();
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
            
            app.UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapGraphQL();
                }); ;

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(x => x.MapControllers());

            app.UseGraphQLVoyager("/graphql-voyager");
        }


        private void AddDbContext(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("SO_Database"))
                .UseLazyLoadingProxies()
            );

            services.AddSingleton(new QueryConnectionString(Configuration.GetConnectionString("SO_ReadonlyDatabase")));
            services.AddTransient<IReadOnlyDatabaseContext, ReadOnlyDatabaseContext>();
        }
    }
}
