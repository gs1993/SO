using Logic.BoundedContexts.Posts.Entities;
using Logic.BoundedContexts.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logic.Utils.Db
{
    public class ReadOnlyDatabaseContext : DbContext
    {
        public DbSet<Post> Posts { get; protected set; }
        public DbSet<User> Users { get; protected set; }

        public ReadOnlyDatabaseContext(DbContextOptions<ReadOnlyDatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DbExtensions.SetupEntites(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                    .AddConsole();
            });

            optionsBuilder
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging();
        }
    }
}
