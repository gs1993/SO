using CdcWorkerService.Db.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace CdcWorkerService.Db
{
    internal class DatabaseContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public DbSet<PostCT> PostsCT { get; protected set; }
        public DbSet<CdcTracking> CdcTrackings { get; protected set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        

        [DbFunction("fn_cdc_get_max_lsn", "sys")]
        public static byte[] GetMaxLsn()
        {
            throw new NotSupportedException("This function can only be used in LINQ-to-Entities query");
        }

        [DbFunction("fn_cdc_get_min_lsn", "sys")]
        public static byte[] GetMinLsn(string captureInstance)
        {
            throw new NotSupportedException("This function can only be used in LINQ-to-Entities query");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostCT>(x =>
            {
                x.ToTable("dbo_Posts_CT", "cdc").HasKey(k => k.Id);
                x.Property(p => p.Operation).HasColumnName("__$operation");
            });

            modelBuilder.Entity<CdcTracking>(x =>
            {
                x.ToTable("CdcTracking").HasKey(k => k.TableName);
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_loggerFactory);
        }
    }
}
