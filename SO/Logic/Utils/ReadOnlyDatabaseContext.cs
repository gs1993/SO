using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Utils
{
    public interface IReadOnlyDatabaseContext
    {
        IQueryable<TEntity> GetQuery<TEntity>() where TEntity : BaseEntity;
        IQueryable<TEntity> GetQueryWithDeleted<TEntity>() where TEntity : BaseEntity;
        Task<TEntity?> GetById<TEntity>(int id, CancellationToken ct = default) where TEntity : BaseEntity;
        Task<TEntity?> GetByIdWithDeleted<TEntity>(int id, CancellationToken ct = default) where TEntity : BaseEntity;
    }

    public class ReadOnlyDatabaseContext : IReadOnlyDatabaseContext
    {
        private readonly DatabaseContext _context;

        public ReadOnlyDatabaseContext(DatabaseContext context, QueryConnectionString queryConnectionString)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _context.Database.SetConnectionString(queryConnectionString.ConnectionString);
        }


        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : BaseEntity
        {
            return _context
                .Set<TEntity>()
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .AsQueryable();
        }

        public IQueryable<TEntity> GetQueryWithDeleted<TEntity>() where TEntity : BaseEntity
        {
            return _context
                .Set<TEntity>()
                .AsNoTracking();
        }

        public async Task<TEntity?> GetById<TEntity>(int id, CancellationToken ct) where TEntity : BaseEntity
        {
            var entity = await _context
                .Set<TEntity>()
                .FindAsync(new object?[] { id }, cancellationToken: ct);

            return entity != null && !entity.IsDeleted
                ? entity
                : null;
        }

        public async Task<TEntity?> GetByIdWithDeleted<TEntity>(int id, CancellationToken ct) where TEntity : BaseEntity
        {
            return await _context
                .Set<TEntity>()
                .FindAsync(new object?[] { id }, cancellationToken: ct);
        }
    }
}
