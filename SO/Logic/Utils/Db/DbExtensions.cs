using Logic.BoundedContexts.Posts.Entities;
using Logic.BoundedContexts.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Utils.Db
{
    public static class DbExtensions
    {
        public static void AddDbContexts(this IServiceCollection services,
            string commandConnectionString, string queryConnectionString)
        {
            services.AddDbContext<DatabaseContext>(options => options
                .UseSqlServer(commandConnectionString,
                    sqlOptions => sqlOptions.CommandTimeout(180))
                .UseLazyLoadingProxies()
            );

            services.AddDbContext<ReadOnlyDatabaseContext>(options => options
                .UseSqlServer(queryConnectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );
        }

        internal static void SetupEntites(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(x =>
            {
                x.ToTable("Posts").HasKey(k => k.Id);
                x.HasQueryFilter(x => !x.IsDeleted);

                x.HasMany(p => p.Comments)
                    .WithOne()
                    .HasForeignKey("PostId");
                x.Metadata.FindNavigation(nameof(Post.Comments))?.SetPropertyAccessMode(PropertyAccessMode.Field);

                x.HasMany(p => p.Votes)
                    .WithOne()
                    .HasForeignKey("PostId");
                x.Metadata.FindNavigation(nameof(Post.Votes))?.SetPropertyAccessMode(PropertyAccessMode.Field);

                x.HasMany(p => p.PostLinks)
                    .WithOne()
                    .HasForeignKey("PostId");
                x.Metadata.FindNavigation(nameof(Post.PostLinks))?.SetPropertyAccessMode(PropertyAccessMode.Field);

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
                x.Property(p => p.Type);
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
                x.OwnsOne(p => p.ProfileInfo, navigationExpression =>
                {
                    navigationExpression.Property(p => p.AboutMe).HasColumnName("AboutMe");
                    navigationExpression.Property(p => p.Age).HasColumnName("Age");
                    navigationExpression.Property(p => p.Location).HasColumnName("Location");
                    navigationExpression.Property(p => p.WebsiteUrl).HasColumnName("WebsiteUrl");
                });
            });
        }
    }
}
