using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Utils
{
    public interface IReadOnlyDatabaseContext
    {
        IQueryable<TEntity> GetQuery<TEntity>() where TEntity : BaseEntity;
        IQueryable<TEntity> GetQueryWithDeleted<TEntity>() where TEntity : BaseEntity;
        Task<TEntity> Get<TEntity>(int id) where TEntity : BaseEntity;
        Task<TEntity> GetWithDeleted<TEntity>(int id) where TEntity : BaseEntity;
    }

    public class ReadOnlyDatabaseContext : IReadOnlyDatabaseContext
    {
        private readonly DatabaseContext _context;

        public ReadOnlyDatabaseContext(DatabaseContext context, QueryConnectionString queryConnectionString)
        {
            _context = context;
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

        public Task<TEntity> Get<TEntity>(int id) where TEntity : BaseEntity
        {
            return _context
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public Task<TEntity> GetWithDeleted<TEntity>(int id) where TEntity : BaseEntity
        {
            return _context
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
