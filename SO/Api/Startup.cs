﻿using Api.Utils;
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
            var connectionString = "Data Source=.;Initial Catalog=StackOverflow2013;Integrated Security=True;";//Configuration["ConnectionString"];
            services.AddDbContext<StackOverflow2010Context>(options =>
                options.UseSqlServer(connectionString));
            services.AddScoped<UnitOfWork>();
            services.AddTransient<PostsRepository>();
            services.AddAutoMapper();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",
                builder => builder.WithOrigins("http://localhost:8100")
                                .AllowAnyMethod()
                                .AllowAnyHeader());
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            //app.UseMiddleware<StackifyMiddleware.RequestTracerMiddleware>(); jebane gówno nie działa....
             
            app.UseCors("AllowMyOrigin");
            app.UseMvc();

            

            //app.UseCors(builder => builder
            //    .WithOrigins(Consts.ApiUrl)
            //    .AllowAnyHeader()
            //    .AllowAnyMethod());


        }
    }
}
