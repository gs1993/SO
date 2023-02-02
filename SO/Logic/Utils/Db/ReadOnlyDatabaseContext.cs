using Logic.BoundedContexts.Posts.Dtos;
using Logic.BoundedContexts.Posts.Entities;
using Logic.BoundedContexts.Users.Dto;
using Logic.BoundedContexts.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logic.Utils.Db
{
    public class ReadOnlyDatabaseContext : DbContext
    {
        public DbSet<PostDto> Posts { get; protected set; }
        public DbSet<UserDto> Users { get; protected set; }

        public ReadOnlyDatabaseContext(DbContextOptions<ReadOnlyDatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostDto>(x =>
            {
                x.ToTable("Posts").HasKey(k => k.Id);
                x.HasQueryFilter(x => !x.IsDeleted);

                x.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey("OwnerUserId");

                x.HasMany(p => p.Comments)
                    .WithOne()
                    .HasForeignKey("PostId");
                x.Metadata.FindNavigation(nameof(PostDto.Comments))?.SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            //modelBuilder.Entity<CommentDto>(x =>
            //{
            //    x.ToTable("Comments").HasKey(k => k.Id);
            //    x.HasQueryFilter(x => !x.IsDeleted);
            //});

            //modelBuilder.Entity<PostType>(x =>
            //{
            //    x.ToTable("PostTypes").HasKey(k => k.Id);
            //    x.Property(p => p.Type);
            //    x.HasQueryFilter(x => !x.IsDeleted);
            //});

            //modelBuilder.Entity<User>(x =>
            //{
            //    x.ToTable("Users").HasKey(k => k.Id);
            //    x.HasQueryFilter(x => !x.IsDeleted);

            //    x.OwnsOne(p => p.VoteSummary, navigationExpression =>
            //    {
            //        navigationExpression.Property(vs => vs.UpVotes).HasColumnName("UpVotes");
            //        navigationExpression.Property(vs => vs.DownVotes).HasColumnName("DownVotes");
            //    });
            //    x.OwnsOne(p => p.ProfileInfo, navigationExpression =>
            //    {
            //        navigationExpression.Property(p => p.AboutMe).HasColumnName("AboutMe");
            //        navigationExpression.Property(p => p.Age).HasColumnName("Age");
            //        navigationExpression.Property(p => p.Location).HasColumnName("Location");
            //        navigationExpression.Property(p => p.WebsiteUrl).HasColumnName("WebsiteUrl");
            //    });
            //});

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
