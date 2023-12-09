using Api;
using ApiGraphQl.GraphQlSchema;
using ApiGraphQl.Utils;
using ElasticSoDatabase.Utils;
using FluentValidation;
using HotChocolate.Subscriptions;
using Logic.Utils;
using Logic.Utils.Db;
using MediatR;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

string commandConnectionString = builder.Configuration.GetConnectionString("SO_Database");
string queryConectionString = builder.Configuration.GetConnectionString("SO_ReadonlyDatabase");

builder.Services.AddDbContexts(commandConnectionString, queryConectionString);

builder.Services.AddElasticsearch(
    builder.Configuration.GetValue<string>("Elasticsearch:Url"),
    builder.Configuration.GetValue<string>("Elasticsearch:DefaultIndexName"));

builder.Services.AddMediatR(
    typeof(DatabaseContext).Assembly,
    typeof(ElasticsearchExtensions).Assembly);

AddFeatureFlags(builder);

AddFluentValidation(builder);

builder.Services
    .AddGraphQLServer()
    .RegisterService<IMediator>()
    .RegisterService<IValidatorFactory>()
    .RegisterService<ITopicEventSender>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddMutationConventions()
    .AddInMemorySubscriptions();

var app = builder.Build();

app.MapGraphQL();

app.Run();


static void AddFeatureFlags(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddSingleton<ITargetingContextAccessor, HttpContextTargetingContextAccessor>();

    builder.Services
        .AddFeatureManagement(builder.Configuration.GetSection("FeatureFlags"))
        .AddFeatureFilter<PercentageFilter>()
        .AddFeatureFilter<TimeWindowFilter>()
        .AddFeatureFilter<TargetingFilter>();
}

static void AddFluentValidation(WebApplicationBuilder builder)
{
    builder.Services.AddValidatorsFromAssemblyContaining<Startup>();
    builder.Services.AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
}