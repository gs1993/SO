using Logic.Read.Posts.Models;
using Logic.Read.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logic.Utils.Db
{
    public class ReadOnlyDatabaseContext : DbContext
    {
        public DbSet<PostModel> Posts { get; protected set; }
        public DbSet<UserModel> Users { get; protected set; }

        public ReadOnlyDatabaseContext(DbContextOptions<ReadOnlyDatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostModel>(x =>
            {
                x.ToTable("Posts").HasKey(k => k.Id);
                x.HasQueryFilter(x => !x.IsDeleted);

                x.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey("OwnerUserId");

                x.HasMany(p => p.Comments)
                    .WithOne()
                    .HasForeignKey("PostId");
            });

            modelBuilder.Entity<CommentModel>(x =>
            {
                x.ToTable("Comments").HasKey(k => k.Id);
                x.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<UserModel>(x =>
            {
                x.ToTable("Users").HasKey(k => k.Id);
                x.HasQueryFilter(x => !x.IsDeleted);
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
    }
}
