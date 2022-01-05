using Logic.Models;
using Logic.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logic.Utils
{
    public class DatabaseContext : DbContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public DbSet<Badges> Badges { get; protected set; }
        public DbSet<Comments> Comments { get; protected set; }
        public DbSet<LinkTypes> LinkTypes { get; protected set; }
        public DbSet<PostLinks> PostLinks { get; protected set; }
        public DbSet<PostTypes> PostTypes { get; protected set; }
        public DbSet<Posts> Posts { get; protected set; }
        public DbSet<Users> Users { get; protected set; }
        public DbSet<VoteTypes> VoteTypes { get; protected set; }
        public DbSet<Votes> Votes { get; protected set; }

        public DatabaseContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
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

        //#region InfrastructureMethods
        //public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        //{
        //    throw new InvalidOperationException("Use attach to add new entity");
        //}

        //public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        //{
        //    throw new InvalidOperationException("Use attach to add new entity");
        //}

        //public override EntityEntry Add(object entity)
        //{
        //    throw new InvalidOperationException("Use attach to add new entity");
        //}

        //public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
        //{
        //    throw new InvalidOperationException("Use attach to add new entity");
        //}

        //public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        //{
        //    (entity as BaseEntity).Delete(_dateTimeProvider.Now);
        //    return null;
        //}

        //public override int SaveChanges()
        //{
        //    var entries = ChangeTracker
        //        .Entries()
        //        .Where(e => e.Entity is BaseEntity
        //           && (e.State == EntityState.Added || e.State == EntityState.Modified));

        //    foreach (var entityEntry in entries)
        //    {
        //        if (entityEntry.State == EntityState.Added)
        //            ((BaseEntity)entityEntry.Entity).SetCreateDate(_dateTimeProvider.Now);
        //        else if (entityEntry.State == EntityState.Modified)
        //            ((BaseEntity)entityEntry.Entity).SetUpdateDate(_dateTimeProvider.Now);
        //    }

        //    return base.SaveChanges();
        //}

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var entries = ChangeTracker
        //        .Entries()
        //        .Where(e => e.Entity is BaseEntity
        //           && (e.State == EntityState.Added || e.State == EntityState.Modified));

        //    var dateNow = _dateTimeProvider.Now;
        //    foreach (var entityEntry in entries)
        //    {
        //        if (entityEntry.State == EntityState.Added)
        //            ((BaseEntity)entityEntry.Entity).SetCreateDate(dateNow);
        //        else if (entityEntry.State == EntityState.Modified)
        //            ((BaseEntity)entityEntry.Entity).SetUpdateDate(dateNow);
        //    }

        //    return base.SaveChangesAsync(cancellationToken);
        //}

        //#endregion
    }
}
