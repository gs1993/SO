using Api.Utils;
using AutoMapper;
using Logic.Repositories;
using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddMvc();

            var connectionString = "Data Source=.;Initial Catalog=StackOverflow2010;Integrated Security=True;";//Configuration["ConnectionString"];
            services.AddDbContext<StackOverflow2010Context>(options =>
                options.UseSqlServer(connectionString));
            services.AddScoped<UnitOfWork>();
            services.AddTransient<PostsRepository>();
            services.AddAutoMapper();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();

            //app.UseCors(builder => builder
            //    .WithOrigins(Consts.ApiUrl)
            //    .AllowAnyHeader()
            //    .AllowAnyMethod());


        }
    }
}
