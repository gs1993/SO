using Logic.BoundedContexts.Posts.Entities;
using Logic.BoundedContexts.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Utils
{
    public class DatabaseContext : DbContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public DbSet<Post> Posts { get; protected set; }
        public DbSet<User> Users { get; protected set; }
       

        public DatabaseContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(x =>
            {
                x.ToTable("Posts").HasKey(k => k.Id);
                x.HasQueryFilter(x => !x.IsDeleted);

                x.HasMany(p => p.Comments)
                    .WithOne()
                    .HasForeignKey("PostId");
                x.Metadata.FindNavigation(nameof(Post.Comments)).SetPropertyAccessMode(PropertyAccessMode.Field);

                x.HasMany(p => p.Votes)
                    .WithOne()
                    .HasForeignKey("PostId");
                x.Metadata.FindNavigation(nameof(Post.Votes)).SetPropertyAccessMode(PropertyAccessMode.Field);

                x.HasMany(p => p.PostLinks)
                    .WithOne()
                    .HasForeignKey("PostId");
                x.Metadata.FindNavigation(nameof(Post.PostLinks)).SetPropertyAccessMode(PropertyAccessMode.Field);

                x.HasOne(p => p.PostType)
                    .WithMany()
                    .HasForeignKey("PostTypeId");
            });

            modelBuilder.Entity<Comment>(x =>
            {
                x.ToTable("Comments").HasKey(k => k.Id);
                x.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<PostType>(x =>
            {
                x.ToTable("PostTypes").HasKey(k => k.Id);
                x.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<User>(x =>
            {
                x.ToTable("Users").HasKey(k => k.Id);
                x.HasQueryFilter(x => !x.IsDeleted);

                x.OwnsOne(p => p.VoteSummary, navigationExpression =>
                {
                    navigationExpression.Property(vs => vs.UpVotes).HasColumnName("UpVotes");
                    navigationExpression.Property(vs => vs.DownVotes).HasColumnName("DownVotes");
                });
            });

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

        #region InfrastructureMethods
        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            throw new InvalidOperationException("Use attach to add new entity");
        }

        public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("Use attach to add new entity");
        }

        public override EntityEntry Add(object entity)
        {
            throw new InvalidOperationException("Use attach to add new entity");
        }

        public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("Use attach to add new entity");
        }

        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            (entity as BaseEntity).Delete(_dateTimeProvider.Now);
            return null;
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity
                   && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                    ((BaseEntity)entityEntry.Entity).SetCreateDate(_dateTimeProvider.Now);
                else if (entityEntry.State == EntityState.Modified)
                    ((BaseEntity)entityEntry.Entity).SetUpdateDate(_dateTimeProvider.Now);
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity
                   && (e.State == EntityState.Added || e.State == EntityState.Modified));

            var dateNow = _dateTimeProvider.Now;
            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                    ((BaseEntity)entityEntry.Entity).SetCreateDate(dateNow);
                else if (entityEntry.State == EntityState.Modified)
                    ((BaseEntity)entityEntry.Entity).SetUpdateDate(dateNow);
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}
