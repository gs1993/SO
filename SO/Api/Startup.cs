using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Api.Controllers;
using MediatR;
using System.Text.Json.Serialization;
using Api.Utils;
using Logic.Utils;

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
            AddDbContext(services);

            services.AddCors();
            services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddSwaggerGen();


            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddHealthChecks()
                .AddCheck<ApiHealthCheck>("api");

            services.AddMediatR(typeof(DatabaseContext).Assembly);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "SO API"));

            app.UseRouting();

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(x => x.MapControllers());
        }


        private void AddDbContext(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("SO_Database");
            services.AddDbContext<DatabaseContext>(options => options
                .UseSqlServer(connectionString)
                .UseLazyLoadingProxies()
            );
            services.AddSingleton(new QueryConnectionString
            {
                ConnectionString = Configuration.GetConnectionString("SO_ReadonlyDatabase")
            });
            services.AddTransient<IReadOnlyDatabaseContext, ReadOnlyDatabaseContext>();
        }
    }
}
