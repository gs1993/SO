using Logic.Utils.Db.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Threading;

namespace Logic.Utils.Db
{
    public static class DbExtensions
    {
        public static void AddDbContexts(this IServiceCollection services, string commandConnectionString, string queryConnectionString)
        {
            services.ConfigureOptions<DatabaseOptionsSetup>();

            services.AddDbContext<DatabaseContext>((serviceProvider, optionsBuilder) =>
            {
                var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;

                optionsBuilder
                    .UseSqlServer(commandConnectionString, sqlOptions => 
                        sqlOptions.CommandTimeout(databaseOptions.CommandTimeoutInSeconds))
                    .UseLazyLoadingProxies();

                optionsBuilder.EnableDetailedErrors(databaseOptions.EnableDatailedErrors);
                optionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            });

            services.AddDbContext<ReadOnlyDatabaseContext>((serviceProvider, optionsBuilder) =>
            {
                var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;

                optionsBuilder
                    .UseSqlServer(queryConnectionString, sqlOptions =>
                        sqlOptions.CommandTimeout(databaseOptions.CommandTimeoutInSeconds))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                optionsBuilder.EnableDetailedErrors(databaseOptions.EnableDatailedErrors);
                optionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            });
        }

        public static ValueTask<T?> FindByIdAsync<T>(this DbSet<T> dbSet, int id, CancellationToken cancellationToken) where T : BaseEntity
            => dbSet.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
    }
}
