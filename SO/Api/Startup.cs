using Api.Mappings;
using Api.Utils;
using AutoMapper;
using Logic.Repositories;
using Microsoft.OpenApi.Models;
using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Api.Controllers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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
            var connectionString = "Data Source=.;Initial Catalog=StackOverflow2013;Integrated Security=True;";//Configuration["ConnectionString"];
            services.AddDbContext<StackOverflow2010Context>(options =>
                options.UseSqlServer(connectionString));
            services.AddScoped<UnitOfWork>();
            services.AddTransient<PostsRepository>();


            services.AddHealthChecks()
                .AddCheck<ApiHealthCheck>("api"); ;

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PostsProfile());
                mc.AddProfile(new UsersProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",
                builder => builder.WithOrigins("http://localhost:8100")
                                .AllowAnyMethod()
                                .AllowAnyHeader());
            });

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();

            app.UseCors("AllowMyOrigin");
            app.UseMvc();

            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = WriteHealthCheckResponse,
                AllowCachingResponses = true
            });

            app.UseSwagger();
        }

        private static Task WriteHealthCheckResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                    new JProperty("status", pair.Value.Status.ToString()),
                    new JProperty("description", pair.Value.Description),
                    new JProperty("data", new JObject(pair.Value.Data.Select(
                    p => new JProperty(p.Key, p.Value))))))))));

            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }
    }
}
